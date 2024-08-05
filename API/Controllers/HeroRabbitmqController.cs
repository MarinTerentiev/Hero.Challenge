using Application.HeroImportPostgressComponent.Commands;
using Application.HeroImportPostgressComponent.Queries;
using Application.RabbitmqPublisher;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HeroRabbitmqController : ApiControllerBase
{
    private readonly IHeroPublisher _messagePublisher;
    private readonly ITextPublisher _textPublisher;
    private readonly IMediator _mediator;
    private readonly ILogger<HeroRabbitmqController> _logger;

    public HeroRabbitmqController(IHeroPublisher messagePublisher, ITextPublisher textPublisher, IMediator mediator,
        ILogger<HeroRabbitmqController> logger)
    {
        _messagePublisher = messagePublisher;
        _textPublisher = textPublisher;
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    [Route("sendtext")]
    public async Task SendText()
    {
        await _textPublisher.Publishe("test text");
    }

    [HttpPost]
    [Route("bulkuploadhero")]
    public async Task<List<int>> BulkUploadHero()
    {
        try
        {
            var formCollection = await Request.ReadFormAsync();
            var file = formCollection.Files.FirstOrDefault();
            if (file == null || file.Length == 0)
            {
                throw new Exception("Failed to read CSV file");
            }

            var fileName = file.FileName;
            var fileExtension = Path.GetExtension(fileName);
            if (fileExtension != ".csv")
            {
                throw new Exception("Invalid file format");
            }

            var failedRows = new List<int>();
            var seedId = Guid.NewGuid();
            var heroes = new List<Hero>();

            //var path = Path.Combine(Directory.GetCurrentDirectory(), "Documents/sample.csv");
            //using (var stream = System.IO.File.OpenRead(path))

            using var stream = file.OpenReadStream();
            using (var parser = new TextFieldParser(stream))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                int i = 0;
                while (!parser.EndOfData)
                {
                    i++;
                    var fields = parser.ReadFields();

                    if (i == 1) continue;

                    if (fields == null)
                    {
                        failedRows.Add(i);
                        continue;
                    }

                    try
                    {
                        var hero = new Hero
                        {
                            Name = fields[0],
                            Class = fields[1],
                            Story = fields[2],
                            Weapon = Enum.TryParse(fields[3], out Weapon myWeapon) ? myWeapon : Weapon.None
                        };

                        await _messagePublisher.Publishe(hero);
                        heroes.Add(hero);
                    }
                    catch (Exception ex)
                    {
                        var data = string.Join(",", fields);
                        var error = $"Failed to parse row {i} for seedid {seedId} for {data}, exception {ex.Message}";
                        _logger.LogError(ex, error);

                        failedRows.Add(i);
                    }
                }
            }

            var heroesImport = heroes.Select(x => new HeroImport
            {
                Class = x.Class,
                Name = x.Name,
                SeedId = seedId,
                Story = x.Story,
                Weapon = x.Weapon,
            }).ToList();
            var command = new BulkInsertHeroImportCommand { Heroes = heroesImport };
            await _mediator.Send(command);

            return failedRows;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to read CSV file" + ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var heroes = await _mediator.Send(new GetBySeedIdHeroImportQuery { SeedId = id });
        return Ok(heroes);
    }
}

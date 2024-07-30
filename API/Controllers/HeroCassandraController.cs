using Application.HeroCassandraComponent.Commands;
using Application.HeroCassandraComponent.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HeroCassandraController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public HeroCassandraController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("HealthCheck")]
    public ActionResult<string> HealthCheck()
    {
        return "Health Check";
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAllHeroes()
    {
        try
        {
            var heroes = await _mediator.Send(new GetAllHeroQuery());
            return Ok(heroes);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var hero = await _mediator.Send(new GetByIdHeroQuery { Id = id });
        if (hero == null)
        {
            return NotFound();
        }

        return Ok(hero);
    }

    [HttpPost("")]
    public async Task<IActionResult> Post([FromBody] Hero hero)
    {
        var ret = await _mediator.Send(new AddHeroCommand { Hero = hero });
        return CreatedAtAction(nameof(Get), new { id = hero.Id }, hero);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateHero(Guid id, [FromBody] Hero hero)
    {
        if (id != hero.Id)
        {
            return BadRequest("Hero ID mismatch");
        }

        var command = new UpdateHeroCommand { Hero = hero };
        await _mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHero(Guid id)
    {
        var command = new DeleteHeroCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }
}

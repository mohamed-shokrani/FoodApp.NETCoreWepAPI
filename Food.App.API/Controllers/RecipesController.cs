﻿using Food.App.API.Filters;
using Food.App.Core.Enums;
using Food.App.Core.Extensions;
using Food.App.Core.Helpers;
using Food.App.Core.Interfaces.Services;
using Food.App.Core.ViewModels.Recipe;
using Food.App.Core.ViewModels.Recipe.Create;
using Food.App.Core.ViewModels.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Food.App.API.Controllers;
[Route("api/recipes/")]
[ApiController]
public class RecipesController : ControllerBase
{
    private readonly IRecipeService _recipeService;

    public RecipesController(IRecipeService recipeService)
    {
        _recipeService = recipeService;
    }
    [HttpGet]
    [Authorize]
    [TypeFilter(typeof(CustomizeAuthorizeAttribute),Arguments =new object[] {Feature.GetRecipes})]
    public async Task<ActionResult<ResponseViewModel<PageList<RecipeViewModel>>>> GetAllRecipes([FromQuery] RecipeParams recipeParams)
    {
        var result = await _recipeService.GetAll(recipeParams);
        Response.AddPaginationHeader(result.Data);
        return Ok(result);
    }
    [HttpGet("admin")]
    public async Task<ActionResult<ResponseViewModel<PageList<RecipeDetailsViewModel>>>> GetAllRecipesAdmin([FromQuery] RecipeParams recipeParams)
    {
        var result = await _recipeService.GetAllForAdmin(recipeParams);
        Response.AddPaginationHeader(result.Data);
        return Ok(result);
    }
    [HttpGet("{id}")]
    public ActionResult<ResponseViewModel<RecipeViewModel>> GetById(int id)
    {
        var result = _recipeService.GetById(id);
        return Ok(result);
    }
    [HttpPost]
    public async Task<ActionResult> Create(CreateRecipeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);

        }
        var result = await _recipeService.Create(model);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        if (result.ErrorCode != ErrorCode.DataBaseError)
        {
            return BadRequest(result);
        }
        return StatusCode((int)ErrorCode.DataBaseError, result.Message);

    }
    [HttpPut]
    public async Task<ActionResult> Update(UpdateRecipeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(model);
        }
        var result = await _recipeService.Update(model);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        if (result.ErrorCode == ErrorCode.RecipeNotFound)
        {
            return NotFound(result);
        }
        return StatusCode((int)ErrorCode.DataBaseError, result.Message);
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        if (!ModelState.IsValid) { }
        var result = await _recipeService.Delete(id);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        if (result.ErrorCode == ErrorCode.RecipeNotFound)
        {
            return NotFound(result);
        }
        return StatusCode((int)ErrorCode.DataBaseError, result.Message);

    }
    [HttpPut("update-image")]
    public async Task<ActionResult> UpdateImage(UpdateReciptImageViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(model);

        }
        var result = await _recipeService.UpdateRecipeImage(model);
        return Ok(result);
    }
}

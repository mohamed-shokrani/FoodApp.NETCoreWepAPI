﻿using AutoMapper.QueryableExtensions;
using Food.App.Core.Entities;
using Food.App.Core.Enums;
using Food.App.Core.Helpers;
using Food.App.Core.Interfaces;
using Food.App.Core.Interfaces.Services;
using Food.App.Core.MappingProfiles;
using Food.App.Core.ViewModels.Recipe;
using Food.App.Core.ViewModels.Response;

namespace Food.App.Service.RecipeService;
public class RecipeService : IRecipeService
{
    private readonly IRepository<Recipe> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public RecipeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repository = unitOfWork.GetRepository<Recipe>();

    }
    public async Task<ResponseViewModel<PageList<RecipeViewModel>>> GetAll(RecipeParams recipeParams)
    {
        var query = _repository.GetAll()
                               .ProjectTo<RecipeViewModel>();
        var paginatedResult = await PageList<RecipeViewModel>.CreateAsync(query, recipeParams.PageNumber, recipeParams.PageSize);

        return new SuccessResponseViewModel<PageList<RecipeViewModel>>(SuccessCode.UsersRetrieved, paginatedResult);
    }
}
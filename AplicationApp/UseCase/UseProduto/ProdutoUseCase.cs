﻿using Aplicacao.Validadores;
using AutoMapper;
using Dominio.Entidades;
using Dominio.Repositorios;
using Dominio.Repositorios.Produto;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.UseProduto;

public class ProdutoUseCase : IProdutoUseCase
{
    private readonly IProdutoWriteOnly _writeOnly;
    private readonly IProdutoReadOnly _readOnly;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProdutoUseCase(IProdutoWriteOnly writeOnly, IProdutoReadOnly readOnly, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _writeOnly = writeOnly;
        _readOnly = readOnly;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseProdutoJson> CrieProduto(RequestProdutoJson request)
    {
        Validate(request);

        var produto = _mapper.Map<Produto>(request);

        await _writeOnly.Add(produto);

        await _unitOfWork.Commit();

        return new ResponseProdutoJson
        {
            Descricao = request.Descricao,
            Disponivel = request.Disponivel,
            Nome = request.Nome,
            Valor = request.Valor
        };
    }

    private static void Validate(RequestProdutoJson request)
    {
        var validator = new ProdutoValidator();

        var result = validator.Validate(request);

        if (result.IsValid == false)
        {
            var mensagensDeErro = result.Errors.Select(e => e.ErrorMessage).ToList();

            throw new ErrorOnValidationException(mensagensDeErro);
        }
    }
}
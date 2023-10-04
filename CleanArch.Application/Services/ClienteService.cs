﻿using CleanArch.Application.Intefaces;
using CleanArch.Domain.Intefaces;
using CleanArch.Domain.Models;
using CleanArch.Domain.Models.Validations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArch.Application.Services
{
    public class ClienteService : BaseService, IClienteService
    {
        private readonly IUnitOfWork _uof;

        public ClienteService(INotificador notificador, IUnitOfWork uof) : base(notificador)
        {
            _uof = uof;
        }

        public async Task Adicionar(Cliente cliente)
        {
            if (!ExecutarValidacao(new ClienteValidacao(), cliente)) return;

            await _uof.ClienteRepository.Adicionar(cliente);
            await _uof.Commit();
        }

        public async Task Atualizar(Cliente cliente)
        {
            if (!ExecutarValidacao(new ClienteValidacao(), cliente)) return;

            await _uof.ClienteRepository.Atualizar(cliente);
            await _uof.Commit();
        }

        public void Inativar(Cliente cliente)
        {
            if (!cliente.EhValido())
                return;

            cliente.Inativar();
            _uof.ClienteRepository.Atualizar(cliente);
            _uof.Commit();
        }

        public async Task Remover(Guid id)
        {
            await _uof.ClienteRepository.Remover(id);
            await _uof.Commit();
        }

        public void Dispose()
        {
            _uof.ClienteRepository?.Dispose();
        }

        public async Task<IEnumerable<Cliente>> ObterTodosAtivos()
        {
            var client = await _uof.ClienteRepository.Buscar(x => x.Ativo);
            return client;
        }
    }
}
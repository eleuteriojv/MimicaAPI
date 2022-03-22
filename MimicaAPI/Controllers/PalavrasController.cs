using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimicaAPI.Data;
using MimicaAPI.Helpers;
using MimicaAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicaAPI.Controllers
{
    [Route("api/palavras")]
    public class PalavrasController : ControllerBase
    {
        private readonly MimicaDbContext _banco;
        public PalavrasController(MimicaDbContext banco)
        {
            _banco = banco;
        }

        //APP -- /api/palavras?data=2022-01-20
        [Route("")]
        [HttpGet]
        public ActionResult ObterTodas(DateTime? data, int? pagNumero, int? pagRegistro)
        {
            var item = _banco.Palavra.AsQueryable();
            if (data.HasValue)
            {
                item = item.Where(a => a.Criado > data.Value || a.Atualizado > data.Value);
                
            }
            if(pagNumero.HasValue){
                var quantidadeTotalRegistros = item.Count();
                item = item.Skip((pagNumero.Value - 1)* pagRegistro.Value).Take(pagRegistro.Value);
                var paginacao = new Paginacao();
                paginacao.NumeroPagina = pagNumero.Value;
                paginacao.RegistroPorPagina = pagRegistro.Value;
                paginacao.TotalRegistros = quantidadeTotalRegistros;
                paginacao.TotalPaginas =(int) Math.Ceiling((double)quantidadeTotalRegistros / pagRegistro.Value);

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginacao));

                if (pagNumero > paginacao.TotalPaginas)
                {
                    return NotFound();
                }
            }
            return Ok(item);
        }

        //WEB
        [Route("{id}")]
        [HttpGet]
        public ActionResult Obter(int id)
        {
            var obj = _banco.Palavra.Find(id);
            if(obj == null)
            {
                return NotFound();
            }
            return Ok(_banco.Palavra.Find(id));
        }

        [Route("")]
        [HttpPost]
        public ActionResult Cadastrar([FromBody]Palavra palavra)
        {
            _banco.Palavra.Add(palavra);
            _banco.SaveChanges();
            return Created($"/api/palavras/{palavra.Id}", palavra);
        }

        [Route("{id}")]
        [HttpPut]
        public ActionResult Atualizar(int id, [FromBody] Palavra palavra)
        {
            var obj = _banco.Palavra.AsNoTracking().FirstOrDefault(x => x.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            palavra.Id = id;
            _banco.Palavra.Update(palavra);
            _banco.SaveChanges();

            return Ok();
        }

        [Route("{id}")]
        [HttpDelete]
        public ActionResult Deletar(int id)
        {
            var palavra = _banco.Palavra.Find(id);
            if (palavra == null)
            {
                return NotFound();
            }

            palavra.Ativo = false;
            _banco.Palavra.Update(palavra);
            _banco.SaveChanges();
            return NoContent();
        }
    }
}

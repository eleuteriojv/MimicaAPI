using Microsoft.AspNetCore.Mvc;
using MimicaAPI.Data;
using MimicaAPI.Models;
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

        //APP
        [Route("")]
        [HttpGet]
        public ActionResult ObterTodas()
        {
            return Ok(_banco.Palavra);
        }

        //WEB
        [Route("{id}")]
        [HttpGet]
        public ActionResult Obter(int id)
        {
            return Ok(_banco.Palavra.Find(id));
        }

        [Route("")]
        [HttpPost]
        public ActionResult Cadastrar([FromBody]Palavra palavra)
        {
            _banco.Palavra.Add(palavra);
            _banco.SaveChanges();
            return Ok();
        }

        [Route("{id}")]
        [HttpPut]
        public ActionResult Atualizar(int id, [FromBody] Palavra palavra)
        {
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
            palavra.Ativo = false;
            _banco.Palavra.Update(palavra);
            _banco.SaveChanges();
            return Ok();
        }
    }
}

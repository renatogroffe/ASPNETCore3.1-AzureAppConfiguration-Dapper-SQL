using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using Dapper;

namespace APITesteConfig.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestesController : ControllerBase
    {
        private static Guid _VALOR_REFERENCIA = Guid.NewGuid();
        private readonly ILogger _logger;

        public TestesController(ILoggerFactory logger)
        {
            _logger = logger.CreateLogger(this.GetType());
        }

        [HttpGet]
        public object Get(
            [FromServices]IConfiguration configuration)
        {
            string conexaoBaseSqlServer =
                configuration.GetConnectionString("BaseSqlServer");
            string valorTesteConfig =
                configuration["ValorTesteConfig"];

            _logger.LogInformation(
                $"ConnectionStrings:BaseSqlServer = {conexaoBaseSqlServer}");
            _logger.LogInformation(
                $"ValorTesteConfig = {valorTesteConfig}");

            using var conexao = new SqlConnection(conexaoBaseSqlServer);

            return new
            {
                Horario = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                VersaoSqlServer = conexao.QueryFirst<string>(
                    "SELECT VersaoSqlServer = @@VERSION"),
                ValorReferencia = _VALOR_REFERENCIA.ToString(),
                ValorTesteConfig = valorTesteConfig
            };
        }
    }
}
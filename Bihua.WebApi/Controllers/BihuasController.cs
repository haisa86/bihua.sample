using Bihua.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bihua.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BihuasController: ControllerBase
    {
        readonly MongoDBContext _dbContext;

        public BihuasController(MongoDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// GET api/Chinese/好
        /// 获取单个中文字符的笔画笔顺信息
        /// </summary>
        /// <param name="text">单个中文字符</param>
        /// <returns></returns>
        [HttpGet("Chinese/{text}")]
        public async Task<ActionResult<ChineseChar>> Chinese(string text)
        {
            if (string.IsNullOrWhiteSpace(text) || text.Length > 1) return BadRequest();

            ushort unicode = text[0];
            return await _dbContext.DbSet<ChineseChar>()
                .Aggregate()
                .FirstOrDefaultAsync(f => f.Unicode == unicode);
        }
    }
}

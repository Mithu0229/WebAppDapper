using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebAppDapper.Models;
using WebAppDapper.Services;

namespace WebAppDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HOmeController : ControllerBase
    {
        private readonly IDapper _dapper;
        public HOmeController(IDapper dapper)
        {
            _dapper = dapper;
        }
        [HttpPost(nameof(Create))]
        public async Task<int> Create(Parameters data)
        {
            var dbparams = new DynamicParameters();
            //dbparams.Add("Id", data.Id, DbType.Int32);
           
            dbparams.Add("Name", data.Name, DbType.String);
            dbparams.Add("Age", data.Age, DbType.Int32);
            var result = await Task.FromResult(_dapper.Insert<int>("[dbo].[SP_Add_Article]"
                , dbparams,
                commandType: CommandType.StoredProcedure));
            return result;
        }
        [HttpGet(nameof(GetById))]
        public async Task<Parameters> GetById(int Id)
        {
            var result = await Task.FromResult(_dapper.Get<Parameters>($"Select * from [Parameters] where Id = {Id}", null, commandType: CommandType.Text));
            return result;
        }
        [HttpDelete(nameof(Delete))]
        public async Task<int> Delete(int Id)
        {
            var result = await Task.FromResult(_dapper.Execute($"Delete [Parameters] Where Id = {Id}", null, commandType: CommandType.Text));
            return result;
        }
        [HttpGet(nameof(Count))]
        public Task<int> Count(int num)
        {
            var totalcount = Task.FromResult(_dapper.Get<int>($"select COUNT(*) from [Parameters] WHERE Age like '%{num}%'", null,
                    commandType: CommandType.Text));
            return totalcount;
        }

        [HttpGet(nameof(GetAll))]
        public Task<List<Parameters>> GetAll()
        {
            var totalcount = Task.FromResult(_dapper.GetAll<Parameters>($"[SelectAllParameters]",null,
                    commandType: CommandType.StoredProcedure));
            return totalcount;
        }
        [HttpPatch(nameof(Update))]
        public Task<int> Update(Parameters data)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Id", data.Id);
            dbPara.Add("Name", data.Name, DbType.String);

            var updateArticle = Task.FromResult(_dapper.Update<int>("[dbo].[SP_Update_Article]",
                            dbPara,
                            commandType: CommandType.StoredProcedure));
            return updateArticle;
        }
    
}
}

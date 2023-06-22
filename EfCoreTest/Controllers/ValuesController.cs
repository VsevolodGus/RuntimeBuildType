using BuildObjects.Builds;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EfCoreTest.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    private readonly TypeFactory _typeFacotory;
    private readonly MyObjectFactory _objectFactory;
    private readonly DataBaseProviderMS _dataBaseProvider;

    public ValuesController(TypeFactory typeFacotory
        , MyObjectFactory objectFactory
        , DataBaseProviderMS dataBaseProvider)
    {
        _typeFacotory = typeFacotory;
        _objectFactory = objectFactory;
        _dataBaseProvider = dataBaseProvider;
    }

    /// <summary>
    /// ALTER PROCEDURE [dbo].[RandomData]
	/// @parametr int
    /// AS
    /// BEGIN
    ///    SET NOCOUNT ON;
    ///    if(@parametr = 1)
    ///		select* from Orders;
    ///	else if(@parametr = 2)
    ///		select* from Books;
    /// END
    /// </summary>
    /// <param name="typeData">@parametr int</param>
    /// <returns></returns>
    [HttpGet]
    public object[] GetData(int typeData)
    {   
        var result = new List<object>();

        // процедура в описание
        var reader = _dataBaseProvider.GetSqlReader($"exec RandomData {typeData}");

        var type = _typeFacotory.CreateType(reader.GetColumnSchema());
        while (reader.Read())
        {
            var obj = _objectFactory.BuildObject(type, reader);
            result.Add(obj);
        }

        _dataBaseProvider.CloseConnection();
        return result.ToArray();
    }
}

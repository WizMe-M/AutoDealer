using AutoDealer.DAL.Database;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoDealer.DAL.Migrations;

[DbContext(typeof(AutoDealerContext))]
[Migration("20230217103000_CreateStoredFunctionsAndTriggers")]
public partial class CreateStoredFunctionsAndTriggers {
}
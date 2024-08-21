using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Migrations.ProductDbCntextMigrations
{
    /// <inheritdoc />
    public partial class CreateGetProductSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                                
                    CREATE OR ALTER PROCEDURE GetProducts
                	@PageIndex int,
                	@PageSize int , 
                	@OrderBy nvarchar(50),
                	@ProductName nvarchar(max) = '%',
                	@ProductDateFrom datetime = NULL,
                	@ProductDateTo datetime = NULL,
                	@Description nvarchar(max) = '%',
                	@CategoryId uniqueidentifier = NULL,
                	@Total int output,
                	@TotalDisplay int output
                AS
                BEGIN

                	SET NOCOUNT ON;

                	Declare @sql nvarchar(2000);
                	Declare @countsql nvarchar(2000);
                	Declare @paramList nvarchar(MAX); 
                	Declare @countparamList nvarchar(MAX);

                	--Collecting TOtal
                	Select @Total = count(*) from products;

                	--Collecting Total Display
                	SET @countsql = 'select @TotalDisplay = count(*) from products pr inner join 
                					Categories c on pr.CategoryId = c.Id where 1 = 1 ';

                	SET @countsql = @countsql + ' AND pr.ProductName LIKE ''%'' + @xProductName + ''%''' 

                	SET @countsql = @countsql + ' AND pr.Description LIKE ''%'' + @xDescription + ''%''' 

                	IF @ProductDateFrom IS NOT NULL
                	SET @countsql = @countsql + ' AND pr.ProductCreateDate >= @xProductDateFrom'

                	IF @ProductDateTo IS NOT NULL
                	SET @countsql = @countsql + ' AND pr.ProductCreateDate <= @xProductDateTo' 

                	IF @CategoryId IS NOT NULL
                	SET @countsql = @countsql + ' AND pr.CategoryId = @xCategoryId' 


                	SELECT @countparamlist = '@xProductName nvarchar(max),
                		@xDescription nvarchar(max),
                		@xProductDateFrom datetime,
                		@xProductDateTo datetime,
                		@xCategoryId uniqueidentifier,
                		@TotalDisplay int output' ;


                	exec sp_executesql @countsql , @countparamlist ,
                		@ProductName,
                		@Description,
                		@ProductDateFrom,
                		@ProductDateTo,
                		@CategoryId,
                		@TotalDisplay = @TotalDisplay output;


                	--Collecting Data
                	SET @sql = 'select * from products pr inner join 
                					Categories c on pr.CategoryId = c.Id where 1 = 1 ';

                	SET @sql = @sql + ' AND pr.ProductName LIKE ''%'' + @xProductName + ''%''' 

                	SET @sql = @sql + ' AND pr.Description LIKE ''%'' + @xDescription + ''%''' 

                	IF @ProductDateFrom IS NOT NULL
                	SET @sql = @sql + ' AND pr.ProductCreateDate >= @xProductDateFrom'

                	IF @ProductDateTo IS NOT NULL
                	SET @sql = @sql + ' AND pr.ProductCreateDate <= @xProductDateTo' 

                	IF @CategoryId IS NOT NULL
                	SET @sql = @sql + ' AND pr.CategoryId = @xCategoryId' 

                	SET @sql = @sql + ' Order by '+@OrderBy+' OFFSET @PageSize * (@PageIndex - 1) 
                	ROWS FETCH NEXT @PageSize ROWS ONLY';


                	SELECT @paramlist = '@xProductName nvarchar(max),
                		@xDescription nvarchar(max),
                		@xProductDateFrom datetime,
                		@xProductDateTo datetime,
                		@xCategoryId uniqueidentifier,
                		@PageIndex int,
                		@PageSize int' ;


                	exec sp_executesql @sql , @paramlist ,
                		@ProductName,
                		@Description,
                		@ProductDateFrom,
                		@ProductDateTo,
                		@CategoryId,
                		@PageIndex,
                		@PageSize;



                END
                GO
                
                """;
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = "DROP PROCEDURE [dbo].[GetProducts]";
            migrationBuilder.DropTable(sql);

        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryApi.Migrations
{
    public partial class updateEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryID",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Suppliers_SupplierID",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_UnitChilds_UnitChildId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_UnitMasterItems_UnitMasterId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Products_ProductID",
                table: "Stocks");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Warehouses_WarehouseID",
                table: "Stocks");

            migrationBuilder.DropForeignKey(
                name: "FK_UnitChilds_UnitMasterItems_UnitMasterId",
                table: "UnitChilds");

            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_BranchItems_BranchId",
                table: "Warehouses");

            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_companies_CompanyId",
                table: "Warehouses");

            migrationBuilder.DropIndex(
                name: "IX_Warehouses_CompanyId",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Warehouses");

            migrationBuilder.AlterColumn<string>(
                name: "BranchId",
                table: "Warehouses",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UnitMasterId",
                table: "Products",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UnitChildId",
                table: "Products",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryID",
                table: "Products",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Suppliers_SupplierID",
                table: "Products",
                column: "SupplierID",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_UnitChilds_UnitChildId",
                table: "Products",
                column: "UnitChildId",
                principalTable: "UnitChilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_UnitMasterItems_UnitMasterId",
                table: "Products",
                column: "UnitMasterId",
                principalTable: "UnitMasterItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Products_ProductID",
                table: "Stocks",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Warehouses_WarehouseID",
                table: "Stocks",
                column: "WarehouseID",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UnitChilds_UnitMasterItems_UnitMasterId",
                table: "UnitChilds",
                column: "UnitMasterId",
                principalTable: "UnitMasterItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_BranchItems_BranchId",
                table: "Warehouses",
                column: "BranchId",
                principalTable: "BranchItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryID",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Suppliers_SupplierID",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_UnitChilds_UnitChildId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_UnitMasterItems_UnitMasterId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Products_ProductID",
                table: "Stocks");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Warehouses_WarehouseID",
                table: "Stocks");

            migrationBuilder.DropForeignKey(
                name: "FK_UnitChilds_UnitMasterItems_UnitMasterId",
                table: "UnitChilds");

            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_BranchItems_BranchId",
                table: "Warehouses");

            migrationBuilder.AlterColumn<string>(
                name: "BranchId",
                table: "Warehouses",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "CompanyId",
                table: "Warehouses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UnitMasterId",
                table: "Products",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UnitChildId",
                table: "Products",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_CompanyId",
                table: "Warehouses",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryID",
                table: "Products",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Suppliers_SupplierID",
                table: "Products",
                column: "SupplierID",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_UnitChilds_UnitChildId",
                table: "Products",
                column: "UnitChildId",
                principalTable: "UnitChilds",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_UnitMasterItems_UnitMasterId",
                table: "Products",
                column: "UnitMasterId",
                principalTable: "UnitMasterItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Products_ProductID",
                table: "Stocks",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Warehouses_WarehouseID",
                table: "Stocks",
                column: "WarehouseID",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UnitChilds_UnitMasterItems_UnitMasterId",
                table: "UnitChilds",
                column: "UnitMasterId",
                principalTable: "UnitMasterItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_BranchItems_BranchId",
                table: "Warehouses",
                column: "BranchId",
                principalTable: "BranchItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_companies_CompanyId",
                table: "Warehouses",
                column: "CompanyId",
                principalTable: "companies",
                principalColumn: "Id");
        }
    }
}

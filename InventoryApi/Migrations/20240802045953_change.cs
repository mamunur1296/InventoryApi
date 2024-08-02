using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryApi.Migrations
{
    public partial class change : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_companies_CompanyId",
                table: "Warehouses");

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_companies_CompanyId",
                table: "Warehouses",
                column: "CompanyId",
                principalTable: "companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_companies_CompanyId",
                table: "Warehouses");

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_companies_CompanyId",
                table: "Warehouses",
                column: "CompanyId",
                principalTable: "companies",
                principalColumn: "Id");
        }
    }
}

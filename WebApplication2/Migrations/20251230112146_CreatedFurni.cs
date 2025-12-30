using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class CreatedFurni : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogTag_Blogs_BlogId",
                table: "BlogTag");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogTag_Tag_TagId",
                table: "BlogTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tag",
                table: "Tag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogTag",
                table: "BlogTag");

            migrationBuilder.RenameTable(
                name: "Tag",
                newName: "Tags");

            migrationBuilder.RenameTable(
                name: "BlogTag",
                newName: "BlogTags");

            migrationBuilder.RenameIndex(
                name: "IX_BlogTag_TagId",
                table: "BlogTags",
                newName: "IX_BlogTags_TagId");

            migrationBuilder.RenameIndex(
                name: "IX_BlogTag_BlogId",
                table: "BlogTags",
                newName: "IX_BlogTags_BlogId");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                table: "Tags",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogTags",
                table: "BlogTags",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeServices_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeServices_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeServices_EmployeeId",
                table: "EmployeeServices",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeServices_ServiceId",
                table: "EmployeeServices",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogTags_Blogs_BlogId",
                table: "BlogTags",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogTags_Tags_TagId",
                table: "BlogTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogTags_Blogs_BlogId",
                table: "BlogTags");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogTags_Tags_TagId",
                table: "BlogTags");

            migrationBuilder.DropTable(
                name: "EmployeeServices");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogTags",
                table: "BlogTags");

            migrationBuilder.RenameTable(
                name: "Tags",
                newName: "Tag");

            migrationBuilder.RenameTable(
                name: "BlogTags",
                newName: "BlogTag");

            migrationBuilder.RenameIndex(
                name: "IX_BlogTags_TagId",
                table: "BlogTag",
                newName: "IX_BlogTag_TagId");

            migrationBuilder.RenameIndex(
                name: "IX_BlogTags_BlogId",
                table: "BlogTag",
                newName: "IX_BlogTag_BlogId");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tag",
                table: "Tag",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogTag",
                table: "BlogTag",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogTag_Blogs_BlogId",
                table: "BlogTag",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogTag_Tag_TagId",
                table: "BlogTag",
                column: "TagId",
                principalTable: "Tag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TRAFO.IO.Migrations
{
    /// <inheritdoc />
    public partial class RemovedLabelsFromTransactionDatabaseEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Labels",
                table: "Transactions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Labels",
                table: "Transactions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}

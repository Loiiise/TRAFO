using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TRAFO.IO.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Amount = table.Column<long>(type: "INTEGER", nullable: false),
                    Currency = table.Column<int>(type: "INTEGER", nullable: false),
                    ThisPartyIdentifier = table.Column<string>(type: "TEXT", nullable: false),
                    ThisPartyName = table.Column<string>(type: "TEXT", nullable: false),
                    OtherPartyIdentifier = table.Column<string>(type: "TEXT", nullable: false),
                    OtherPartyName = table.Column<string>(type: "TEXT", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PaymentReference = table.Column<string>(type: "TEXT", nullable: false),
                    BIC = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    RawData = table.Column<string>(type: "TEXT", nullable: false),
                    PrimairyLabel = table.Column<string>(type: "TEXT", nullable: false),
                    Labels = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");
        }
    }
}

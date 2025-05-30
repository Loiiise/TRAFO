using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TRAFO.IO.Migrations
{
    /// <inheritdoc />
    public partial class RedidDatabaseTransactionRelevantEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.CreateTable(
                name: "AccountBalanceDatabaseEntry",
                columns: table => new
                {
                    BalanceId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AccountId = table.Column<string>(type: "TEXT", nullable: false),
                    Amount = table.Column<long>(type: "INTEGER", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDerived = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountBalanceDatabaseEntry", x => x.BalanceId);
                });

            migrationBuilder.CreateTable(
                name: "Label",
                columns: table => new
                {
                    LabelId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ParentLabelId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    LabelCategoryId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    DisplayId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Label", x => x.LabelId);
                });

            migrationBuilder.CreateTable(
                name: "LabelBalance",
                columns: table => new
                {
                    BalanceId = table.Column<Guid>(type: "TEXT", nullable: false),
                    LabelId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Amount = table.Column<long>(type: "INTEGER", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDerived = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabelBalance", x => x.BalanceId);
                });

            migrationBuilder.CreateTable(
                name: "LabelCategorie",
                columns: table => new
                {
                    LabelCategoryId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabelCategorie", x => x.LabelCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "LabelCategoryBalance",
                columns: table => new
                {
                    BalanceId = table.Column<Guid>(type: "TEXT", nullable: false),
                    LabelCategoryId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Amount = table.Column<long>(type: "INTEGER", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDerived = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabelCategoryBalance", x => x.BalanceId);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    TransactionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ParentTransactionId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Amount = table.Column<long>(type: "INTEGER", nullable: false),
                    ThisPartyAccountId = table.Column<string>(type: "TEXT", nullable: false),
                    ThisPartyName = table.Column<string>(type: "TEXT", nullable: true),
                    OtherPartyAccountId = table.Column<string>(type: "TEXT", nullable: false),
                    OtherPartyName = table.Column<string>(type: "TEXT", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PaymentReference = table.Column<string>(type: "TEXT", nullable: true),
                    BIC = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    RawData = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.TransactionId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountBalanceDatabaseEntry");

            migrationBuilder.DropTable(
                name: "Label");

            migrationBuilder.DropTable(
                name: "LabelBalance");

            migrationBuilder.DropTable(
                name: "LabelCategorie");

            migrationBuilder.DropTable(
                name: "LabelCategoryBalance");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Amount = table.Column<long>(type: "INTEGER", nullable: false),
                    BIC = table.Column<string>(type: "TEXT", nullable: false),
                    Currency = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    OtherPartyIdentifier = table.Column<string>(type: "TEXT", nullable: false),
                    OtherPartyName = table.Column<string>(type: "TEXT", nullable: false),
                    PaymentReference = table.Column<string>(type: "TEXT", nullable: false),
                    PrimairyLabel = table.Column<string>(type: "TEXT", nullable: false),
                    RawData = table.Column<string>(type: "TEXT", nullable: false),
                    ThisPartyIdentifier = table.Column<string>(type: "TEXT", nullable: false),
                    ThisPartyName = table.Column<string>(type: "TEXT", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                });
        }
    }
}

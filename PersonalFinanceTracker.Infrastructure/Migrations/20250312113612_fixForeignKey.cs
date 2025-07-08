using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalFinanceTracker.Infrastructure.Migrations

{
    /// <inheritdoc />
    public partial class fixForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_Categories_CategoriesCategory_Id",
                table: "Budgets");

            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_Users_UsersUser_Id",
                table: "Budgets");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Users_UsersUser_Id",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Categories_CategoriesCategory_Id",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Users_UsersUser_Id",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_CategoriesCategory_Id",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_UsersUser_Id",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Categories_UsersUser_Id",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Budgets_CategoriesCategory_Id",
                table: "Budgets");

            migrationBuilder.DropIndex(
                name: "IX_Budgets_UsersUser_Id",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "CategoriesCategory_Id",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "UsersUser_Id",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "UsersUser_Id",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CategoriesCategory_Id",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "UsersUser_Id",
                table: "Budgets");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_Category_Id",
                table: "Transactions",
                column: "Category_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_User_Id",
                table: "Transactions",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_User_Id",
                table: "Categories",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_Category_Id",
                table: "Budgets",
                column: "Category_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_User_Id",
                table: "Budgets",
                column: "User_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Budgets_Categories_Category_Id",
                table: "Budgets",
                column: "Category_Id",
                principalTable: "Categories",
                principalColumn: "Category_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Budgets_Users_User_Id",
                table: "Budgets",
                column: "User_Id",
                principalTable: "Users",
                principalColumn: "User_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Users_User_Id",
                table: "Categories",
                column: "User_Id",
                principalTable: "Users",
                principalColumn: "User_Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Categories_Category_Id",
                table: "Transactions",
                column: "Category_Id",
                principalTable: "Categories",
                principalColumn: "Category_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Users_User_Id",
                table: "Transactions",
                column: "User_Id",
                principalTable: "Users",
                principalColumn: "User_Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_Categories_Category_Id",
                table: "Budgets");

            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_Users_User_Id",
                table: "Budgets");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Users_User_Id",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Categories_Category_Id",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Users_User_Id",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_Category_Id",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_User_Id",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Categories_User_Id",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Budgets_Category_Id",
                table: "Budgets");

            migrationBuilder.DropIndex(
                name: "IX_Budgets_User_Id",
                table: "Budgets");

            migrationBuilder.AddColumn<int>(
                name: "CategoriesCategory_Id",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UsersUser_Id",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UsersUser_Id",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CategoriesCategory_Id",
                table: "Budgets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UsersUser_Id",
                table: "Budgets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CategoriesCategory_Id",
                table: "Transactions",
                column: "CategoriesCategory_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UsersUser_Id",
                table: "Transactions",
                column: "UsersUser_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_UsersUser_Id",
                table: "Categories",
                column: "UsersUser_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_CategoriesCategory_Id",
                table: "Budgets",
                column: "CategoriesCategory_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_UsersUser_Id",
                table: "Budgets",
                column: "UsersUser_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Budgets_Categories_CategoriesCategory_Id",
                table: "Budgets",
                column: "CategoriesCategory_Id",
                principalTable: "Categories",
                principalColumn: "Category_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Budgets_Users_UsersUser_Id",
                table: "Budgets",
                column: "UsersUser_Id",
                principalTable: "Users",
                principalColumn: "User_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Users_UsersUser_Id",
                table: "Categories",
                column: "UsersUser_Id",
                principalTable: "Users",
                principalColumn: "User_Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Categories_CategoriesCategory_Id",
                table: "Transactions",
                column: "CategoriesCategory_Id",
                principalTable: "Categories",
                principalColumn: "Category_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Users_UsersUser_Id",
                table: "Transactions",
                column: "UsersUser_Id",
                principalTable: "Users",
                principalColumn: "User_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

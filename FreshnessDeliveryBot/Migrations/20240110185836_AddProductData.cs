using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreshnessDeliveryBot.Migrations
{
    public partial class AddProductData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Name", "Description", "Price" },
                values: new object[,]
                {
            { "Новий бутель", "Бутель (18,9 л.) + вода", 350.00m },
            { "Помпа механічна", "Матеріал корпусу: харчовий поліпропілен. Матеріал пружини: пластик. Кріплення на сулію: засувка. Тип трубки: збірна.", 150.00m },
            { "Помпа електрична", "Матеріал корпусу: пластик. Комплектація: занурювальний шланг з харчового ПВХ, трубка, що дозує, з харчової нержавіючої сталі, USB-кабель, коробка.", 300.00m },
            { "Підставка під бутель", "Підставка складається з двох хрестовин, які забезпечують надійність та стійкість. Матеріал: сосна. Розміри: 100-490мм.", 350.00m },
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValues: new object[] { 1, 2, 3, 4 });
        }

    }
}

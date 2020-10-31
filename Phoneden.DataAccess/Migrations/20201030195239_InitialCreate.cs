using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Phoneden.DataAccess.Migrations
{
  public partial class InitialCreate : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "asp_net_roles",
          columns: table => new
          {
            id = table.Column<string>(nullable: false),
            name = table.Column<string>(maxLength: 127, nullable: true),
            normalized_name = table.Column<string>(maxLength: 127, nullable: true),
            concurrency_stamp = table.Column<string>(nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_asp_net_roles", x => x.id);
          });

      migrationBuilder.CreateTable(
          name: "asp_net_users",
          columns: table => new
          {
            id = table.Column<string>(nullable: false),
            user_name = table.Column<string>(maxLength: 127, nullable: true),
            normalized_user_name = table.Column<string>(maxLength: 127, nullable: true),
            email = table.Column<string>(maxLength: 127, nullable: true),
            normalized_email = table.Column<string>(maxLength: 127, nullable: true),
            email_confirmed = table.Column<bool>(nullable: false),
            password_hash = table.Column<string>(nullable: true),
            security_stamp = table.Column<string>(nullable: true),
            concurrency_stamp = table.Column<string>(nullable: true),
            phone_number = table.Column<string>(nullable: true),
            phone_number_confirmed = table.Column<bool>(nullable: false),
            two_factor_enabled = table.Column<bool>(nullable: false),
            lockout_end = table.Column<DateTimeOffset>(nullable: true),
            lockout_enabled = table.Column<bool>(nullable: false),
            access_failed_count = table.Column<int>(nullable: false),
            display_username = table.Column<string>(nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_asp_net_users", x => x.id);
          });

      migrationBuilder.CreateTable(
          name: "brands",
          columns: table => new
          {
            id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            name = table.Column<string>(nullable: true),
            is_deleted = table.Column<bool>(nullable: false),
            created_on = table.Column<DateTime>(nullable: false),
            modified_on = table.Column<DateTime>(nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_brands", x => x.id);
          });

      migrationBuilder.CreateTable(
          name: "businesses",
          columns: table => new
          {
            id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            name = table.Column<string>(nullable: true),
            code = table.Column<string>(nullable: true),
            description = table.Column<string>(nullable: true),
            phone = table.Column<string>(nullable: true),
            website = table.Column<string>(nullable: true),
            email = table.Column<string>(nullable: true),
            is_deleted = table.Column<bool>(nullable: false),
            created_on = table.Column<DateTime>(nullable: false),
            modified_on = table.Column<DateTime>(nullable: true),
            discriminator = table.Column<string>(nullable: false),
            allowed_credit = table.Column<decimal>(type: "decimal(19, 8)", nullable: true),
            credit_used = table.Column<decimal>(type: "decimal(19, 8)", nullable: true),
            number_of_days_allowed_to_be_on_maxed_out_credit = table.Column<int>(nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_businesses", x => x.id);
          });

      migrationBuilder.CreateTable(
          name: "categories",
          columns: table => new
          {
            id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            name = table.Column<string>(nullable: true),
            is_deleted = table.Column<bool>(nullable: false),
            created_on = table.Column<DateTime>(nullable: false),
            modified_on = table.Column<DateTime>(nullable: true),
            parent_category_id = table.Column<int>(nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_categories", x => x.id);
            table.ForeignKey(
                      name: "fk_categories_categories_parent_category_id",
                      column: x => x.parent_category_id,
                      principalTable: "categories",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateTable(
          name: "partners",
          columns: table => new
          {
            id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            title = table.Column<string>(nullable: true),
            first_name = table.Column<string>(nullable: true),
            last_name = table.Column<string>(nullable: true),
            phone = table.Column<string>(nullable: true),
            email = table.Column<string>(nullable: true),
            is_deleted = table.Column<bool>(nullable: false),
            created_on = table.Column<DateTime>(nullable: false),
            modified_on = table.Column<DateTime>(nullable: true),
            address_line1 = table.Column<string>(nullable: true),
            address_line2 = table.Column<string>(nullable: true),
            area = table.Column<string>(nullable: true),
            city = table.Column<string>(nullable: true),
            county = table.Column<string>(nullable: true),
            post_code = table.Column<string>(nullable: true),
            country = table.Column<string>(nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_partners", x => x.id);
          });

      migrationBuilder.CreateTable(
          name: "qualities",
          columns: table => new
          {
            id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            name = table.Column<string>(nullable: true),
            is_deleted = table.Column<bool>(nullable: false),
            created_on = table.Column<DateTime>(nullable: false),
            modified_on = table.Column<DateTime>(nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_qualities", x => x.id);
          });

      migrationBuilder.CreateTable(
          name: "asp_net_role_claims",
          columns: table => new
          {
            id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            role_id = table.Column<string>(nullable: false),
            claim_type = table.Column<string>(nullable: true),
            claim_value = table.Column<string>(nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_asp_net_role_claims", x => x.id);
            table.ForeignKey(
                      name: "fk_asp_net_role_claims_asp_net_roles_role_id",
                      column: x => x.role_id,
                      principalTable: "asp_net_roles",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "asp_net_user_claims",
          columns: table => new
          {
            id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            user_id = table.Column<string>(nullable: false),
            claim_type = table.Column<string>(nullable: true),
            claim_value = table.Column<string>(nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_asp_net_user_claims", x => x.id);
            table.ForeignKey(
                      name: "fk_asp_net_user_claims_asp_net_users_user_id",
                      column: x => x.user_id,
                      principalTable: "asp_net_users",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "asp_net_user_logins",
          columns: table => new
          {
            login_provider = table.Column<string>(maxLength: 127, nullable: false),
            provider_key = table.Column<string>(maxLength: 127, nullable: false),
            provider_display_name = table.Column<string>(nullable: true),
            user_id = table.Column<string>(nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_asp_net_user_logins", x => new { x.login_provider, x.provider_key });
            table.ForeignKey(
                      name: "fk_asp_net_user_logins_asp_net_users_user_id",
                      column: x => x.user_id,
                      principalTable: "asp_net_users",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "asp_net_user_roles",
          columns: table => new
          {
            user_id = table.Column<string>(maxLength: 127, nullable: false),
            role_id = table.Column<string>(maxLength: 127, nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_asp_net_user_roles", x => new { x.user_id, x.role_id });
            table.ForeignKey(
                      name: "fk_asp_net_user_roles_asp_net_roles_role_id",
                      column: x => x.role_id,
                      principalTable: "asp_net_roles",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "fk_asp_net_user_roles_asp_net_users_user_id",
                      column: x => x.user_id,
                      principalTable: "asp_net_users",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "asp_net_user_tokens",
          columns: table => new
          {
            user_id = table.Column<string>(maxLength: 127, nullable: false),
            login_provider = table.Column<string>(maxLength: 127, nullable: false),
            name = table.Column<string>(maxLength: 127, nullable: false),
            value = table.Column<string>(nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_asp_net_user_tokens", x => new { x.user_id, x.login_provider, x.name });
            table.ForeignKey(
                      name: "fk_asp_net_user_tokens_asp_net_users_user_id",
                      column: x => x.user_id,
                      principalTable: "asp_net_users",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "expenses",
          columns: table => new
          {
            id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            date = table.Column<DateTime>(nullable: false),
            method = table.Column<int>(nullable: false),
            amount = table.Column<decimal>(type: "decimal(19, 8)", nullable: false),
            reason = table.Column<string>(nullable: true),
            application_user_id = table.Column<string>(nullable: true),
            is_deleted = table.Column<bool>(nullable: false),
            created_on = table.Column<DateTime>(nullable: false),
            modified_on = table.Column<DateTime>(nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_expenses", x => x.id);
            table.ForeignKey(
                      name: "fk_expenses_asp_net_users_application_user_id",
                      column: x => x.application_user_id,
                      principalTable: "asp_net_users",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateTable(
          name: "addresses",
          columns: table => new
          {
            id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            address_line1 = table.Column<string>(nullable: true),
            address_line2 = table.Column<string>(nullable: true),
            area = table.Column<string>(nullable: true),
            city = table.Column<string>(nullable: true),
            county = table.Column<string>(nullable: true),
            post_code = table.Column<string>(nullable: true),
            country = table.Column<string>(nullable: true),
            is_deleted = table.Column<bool>(nullable: false),
            created_on = table.Column<DateTime>(nullable: false),
            modified_on = table.Column<DateTime>(nullable: true),
            business_id = table.Column<int>(nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_addresses", x => x.id);
            table.ForeignKey(
                      name: "fk_addresses_businesses_business_id",
                      column: x => x.business_id,
                      principalTable: "businesses",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "contacts",
          columns: table => new
          {
            id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            title = table.Column<string>(nullable: true),
            first_name = table.Column<string>(nullable: true),
            last_name = table.Column<string>(nullable: true),
            phone = table.Column<string>(nullable: true),
            email = table.Column<string>(nullable: true),
            is_deleted = table.Column<bool>(nullable: false),
            created_on = table.Column<DateTime>(nullable: false),
            modified_on = table.Column<DateTime>(nullable: true),
            department = table.Column<string>(nullable: true),
            business_id = table.Column<int>(nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_contacts", x => x.id);
            table.ForeignKey(
                      name: "fk_contacts_businesses_business_id",
                      column: x => x.business_id,
                      principalTable: "businesses",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "purchase_orders",
          columns: table => new
          {
            id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            date = table.Column<DateTime>(nullable: false),
            shipping_cost = table.Column<decimal>(type: "decimal(19, 8)", nullable: false),
            shipping_currency = table.Column<int>(nullable: false),
            shipping_conversion_rate = table.Column<decimal>(type: "decimal(19, 8)", nullable: false),
            discount = table.Column<decimal>(type: "decimal(19, 8)", nullable: false),
            supplier_order_number = table.Column<int>(nullable: false),
            status = table.Column<int>(nullable: false),
            vat = table.Column<decimal>(type: "decimal(19, 8)", nullable: false),
            import_duty = table.Column<decimal>(type: "decimal(19, 8)", nullable: false),
            supplier_id = table.Column<int>(nullable: false),
            is_deleted = table.Column<bool>(nullable: false),
            created_on = table.Column<DateTime>(nullable: false),
            modified_on = table.Column<DateTime>(nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_purchase_orders", x => x.id);
            table.ForeignKey(
                      name: "fk_purchase_orders_businesses_supplier_id",
                      column: x => x.supplier_id,
                      principalTable: "businesses",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateTable(
          name: "sale_orders",
          columns: table => new
          {
            id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            date = table.Column<DateTime>(nullable: false),
            postage_cost = table.Column<decimal>(type: "decimal(19, 8)", nullable: false),
            discount = table.Column<decimal>(type: "decimal(19, 8)", nullable: false),
            status = table.Column<int>(nullable: false),
            customer_id = table.Column<int>(nullable: false),
            is_deleted = table.Column<bool>(nullable: false),
            created_on = table.Column<DateTime>(nullable: false),
            modified_on = table.Column<DateTime>(nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_sale_orders", x => x.id);
            table.ForeignKey(
                      name: "fk_sale_orders_businesses_customer_id",
                      column: x => x.customer_id,
                      principalTable: "businesses",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateTable(
          name: "products",
          columns: table => new
          {
            id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            name = table.Column<string>(nullable: true),
            sku = table.Column<string>(nullable: true),
            barcode = table.Column<string>(nullable: true),
            description = table.Column<string>(nullable: true),
            colour = table.Column<int>(nullable: false),
            quantity = table.Column<int>(nullable: false),
            unit_cost_price = table.Column<decimal>(type: "decimal(19, 8)", nullable: false),
            unit_selling_price = table.Column<decimal>(type: "decimal(19, 8)", nullable: false),
            alert_threshold = table.Column<int>(nullable: false),
            image_path = table.Column<string>(nullable: true),
            is_deleted = table.Column<bool>(nullable: false),
            created_on = table.Column<DateTime>(nullable: false),
            modified_on = table.Column<DateTime>(nullable: true),
            category_id = table.Column<int>(nullable: false),
            brand_id = table.Column<int>(nullable: false),
            quality_id = table.Column<int>(nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_products", x => x.id);
            table.ForeignKey(
                      name: "fk_products_brands_brand_id",
                      column: x => x.brand_id,
                      principalTable: "brands",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "fk_products_categories_category_id",
                      column: x => x.category_id,
                      principalTable: "categories",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "fk_products_qualities_quality_id",
                      column: x => x.quality_id,
                      principalTable: "qualities",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "purchase_order_invoices",
          columns: table => new
          {
            id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            amount = table.Column<decimal>(type: "decimal(19, 8)", nullable: false),
            due_date = table.Column<DateTime>(nullable: false),
            is_deleted = table.Column<bool>(nullable: false),
            status = table.Column<int>(nullable: false),
            created_on = table.Column<DateTime>(nullable: false),
            modified_on = table.Column<DateTime>(nullable: true),
            purchase_order_id = table.Column<int>(nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_purchase_order_invoices", x => x.id);
            table.ForeignKey(
                      name: "fk_purchase_order_invoices_purchase_orders_purchase_order_id",
                      column: x => x.purchase_order_id,
                      principalTable: "purchase_orders",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "purchase_order_note",
          columns: table => new
          {
            id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            text = table.Column<string>(nullable: true),
            is_deleted = table.Column<bool>(nullable: false),
            created_on = table.Column<DateTime>(nullable: false),
            modified_on = table.Column<DateTime>(nullable: true),
            purchase_order_id = table.Column<int>(nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_purchase_order_note", x => x.id);
            table.ForeignKey(
                      name: "fk_purchase_order_note_purchase_orders_purchase_order_id",
                      column: x => x.purchase_order_id,
                      principalTable: "purchase_orders",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "sale_order_invoices",
          columns: table => new
          {
            id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            amount = table.Column<decimal>(type: "decimal(19, 8)", nullable: false),
            due_date = table.Column<DateTime>(nullable: false),
            is_deleted = table.Column<bool>(nullable: false),
            status = table.Column<int>(nullable: false),
            amount_to_be_paid_on_credit = table.Column<decimal>(type: "decimal(19, 8)", nullable: false),
            created_on = table.Column<DateTime>(nullable: false),
            modified_on = table.Column<DateTime>(nullable: true),
            sale_order_id = table.Column<int>(nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_sale_order_invoices", x => x.id);
            table.ForeignKey(
                      name: "fk_sale_order_invoices_sale_orders_sale_order_id",
                      column: x => x.sale_order_id,
                      principalTable: "sale_orders",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "sale_order_note",
          columns: table => new
          {
            id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            text = table.Column<string>(nullable: true),
            is_deleted = table.Column<bool>(nullable: false),
            created_on = table.Column<DateTime>(nullable: false),
            modified_on = table.Column<DateTime>(nullable: true),
            purchase_order_id = table.Column<int>(nullable: false),
            sale_order_id = table.Column<int>(nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_sale_order_note", x => x.id);
            table.ForeignKey(
                      name: "fk_sale_order_note_purchase_orders_purchase_order_id",
                      column: x => x.purchase_order_id,
                      principalTable: "purchase_orders",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "fk_sale_order_note_sale_orders_sale_order_id",
                      column: x => x.sale_order_id,
                      principalTable: "sale_orders",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateTable(
          name: "purchase_order_line_items",
          columns: table => new
          {
            id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            name = table.Column<string>(nullable: true),
            price = table.Column<decimal>(type: "decimal(19, 8)", nullable: false),
            cost = table.Column<decimal>(nullable: false),
            currency = table.Column<int>(nullable: false),
            conversion_rate = table.Column<decimal>(type: "decimal(19, 8)", nullable: false),
            quality = table.Column<string>(nullable: true),
            colour = table.Column<string>(nullable: true),
            quantity = table.Column<int>(nullable: false),
            is_deleted = table.Column<bool>(nullable: false),
            created_on = table.Column<DateTime>(nullable: false),
            modified_on = table.Column<DateTime>(nullable: true),
            purchase_order_id = table.Column<int>(nullable: false),
            product_id = table.Column<int>(nullable: false),
            barcode = table.Column<string>(nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_purchase_order_line_items", x => x.id);
            table.ForeignKey(
                      name: "fk_purchase_order_line_items_products_product_id",
                      column: x => x.product_id,
                      principalTable: "products",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "fk_purchase_order_line_items_purchase_orders_purchase_order_id",
                      column: x => x.purchase_order_id,
                      principalTable: "purchase_orders",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateTable(
          name: "sale_order_line_items",
          columns: table => new
          {
            id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            name = table.Column<string>(nullable: true),
            price = table.Column<decimal>(type: "decimal(19, 8)", nullable: false),
            cost = table.Column<decimal>(nullable: false),
            currency = table.Column<int>(nullable: false),
            conversion_rate = table.Column<decimal>(type: "decimal(19, 8)", nullable: false),
            quality = table.Column<string>(nullable: true),
            colour = table.Column<string>(nullable: true),
            quantity = table.Column<int>(nullable: false),
            is_deleted = table.Column<bool>(nullable: false),
            created_on = table.Column<DateTime>(nullable: false),
            modified_on = table.Column<DateTime>(nullable: true),
            sale_order_id = table.Column<int>(nullable: false),
            product_id = table.Column<int>(nullable: false),
            barcode = table.Column<string>(nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_sale_order_line_items", x => x.id);
            table.ForeignKey(
                      name: "fk_sale_order_line_items_products_product_id",
                      column: x => x.product_id,
                      principalTable: "products",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "fk_sale_order_line_items_sale_orders_sale_order_id",
                      column: x => x.sale_order_id,
                      principalTable: "sale_orders",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateTable(
          name: "purchase_order_invoice_line_items",
          columns: table => new
          {
            id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            product_id = table.Column<int>(nullable: false),
            product_name = table.Column<string>(nullable: true),
            product_colour = table.Column<string>(nullable: true),
            product_quality = table.Column<string>(nullable: true),
            quantity = table.Column<int>(nullable: false),
            price = table.Column<decimal>(nullable: false),
            currency = table.Column<int>(nullable: false),
            conversion_rate = table.Column<decimal>(nullable: false),
            purchase_order_invoice_id = table.Column<int>(nullable: false),
            is_deleted = table.Column<bool>(nullable: false),
            created_on = table.Column<DateTime>(nullable: false),
            modified_on = table.Column<DateTime>(nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_purchase_order_invoice_line_items", x => x.id);
            table.ForeignKey(
                      name: "fk_purchase_order_invoice_line_items_purchase_order_invoices_p~",
                      column: x => x.purchase_order_invoice_id,
                      principalTable: "purchase_order_invoices",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "purchase_order_invoice_payments",
          columns: table => new
          {
            id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            amount = table.Column<decimal>(type: "decimal(19, 8)", nullable: false),
            method = table.Column<int>(nullable: false),
            currency = table.Column<int>(nullable: false),
            conversion_rate = table.Column<decimal>(type: "decimal(19, 8)", nullable: false),
            reference = table.Column<string>(nullable: true),
            date = table.Column<DateTime>(nullable: false),
            purchase_order_invoice_id = table.Column<int>(nullable: false),
            is_deleted = table.Column<bool>(nullable: false),
            created_on = table.Column<DateTime>(nullable: false),
            modified_on = table.Column<DateTime>(nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_purchase_order_invoice_payments", x => x.id);
            table.ForeignKey(
                      name: "fk_purchase_order_invoice_payments_purchase_order_invoices_pur~",
                      column: x => x.purchase_order_invoice_id,
                      principalTable: "purchase_order_invoices",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "sale_order_invoice_line_items",
          columns: table => new
          {
            id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            product_id = table.Column<int>(nullable: false),
            product_name = table.Column<string>(nullable: true),
            product_colour = table.Column<string>(nullable: true),
            product_quality = table.Column<string>(nullable: true),
            quantity = table.Column<int>(nullable: false),
            price = table.Column<decimal>(nullable: false),
            cost = table.Column<decimal>(nullable: false),
            sale_order_invoice_id = table.Column<int>(nullable: false),
            is_deleted = table.Column<bool>(nullable: false),
            created_on = table.Column<DateTime>(nullable: false),
            modified_on = table.Column<DateTime>(nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_sale_order_invoice_line_items", x => x.id);
            table.ForeignKey(
                      name: "fk_sale_order_invoice_line_items_sale_order_invoices_sale_orde~",
                      column: x => x.sale_order_invoice_id,
                      principalTable: "sale_order_invoices",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "sale_order_invoice_payments",
          columns: table => new
          {
            id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            amount = table.Column<decimal>(type: "decimal(19, 8)", nullable: false),
            method = table.Column<int>(nullable: false),
            reference = table.Column<string>(nullable: true),
            date = table.Column<DateTime>(nullable: false),
            sale_order_invoice_id = table.Column<int>(nullable: false),
            is_deleted = table.Column<bool>(nullable: false),
            created_on = table.Column<DateTime>(nullable: false),
            modified_on = table.Column<DateTime>(nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_sale_order_invoice_payments", x => x.id);
            table.ForeignKey(
                      name: "fk_sale_order_invoice_payments_sale_order_invoices_sale_order_~",
                      column: x => x.sale_order_invoice_id,
                      principalTable: "sale_order_invoices",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "sale_order_returns",
          columns: table => new
          {
            id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            date = table.Column<DateTime>(nullable: false),
            quantity = table.Column<int>(nullable: false),
            resolution = table.Column<int>(nullable: false),
            description = table.Column<string>(nullable: true),
            product_id = table.Column<int>(nullable: false),
            value = table.Column<decimal>(type: "decimal(19, 8)", nullable: false),
            is_verified = table.Column<bool>(nullable: false),
            sale_order_invoice_id = table.Column<int>(nullable: false),
            is_deleted = table.Column<bool>(nullable: false),
            created_on = table.Column<DateTime>(nullable: false),
            modified_on = table.Column<DateTime>(nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_sale_order_returns", x => x.id);
            table.ForeignKey(
                      name: "fk_sale_order_returns_products_product_id",
                      column: x => x.product_id,
                      principalTable: "products",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "fk_sale_order_returns_sale_order_invoices_sale_order_invoice_id",
                      column: x => x.sale_order_invoice_id,
                      principalTable: "sale_order_invoices",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateIndex(
          name: "ix_addresses_business_id",
          table: "addresses",
          column: "business_id");

      migrationBuilder.CreateIndex(
          name: "ix_asp_net_role_claims_role_id",
          table: "asp_net_role_claims",
          column: "role_id");

      migrationBuilder.CreateIndex(
          name: "role_name_index",
          table: "asp_net_roles",
          column: "normalized_name",
          unique: true);

      migrationBuilder.CreateIndex(
          name: "ix_asp_net_user_claims_user_id",
          table: "asp_net_user_claims",
          column: "user_id");

      migrationBuilder.CreateIndex(
          name: "ix_asp_net_user_logins_user_id",
          table: "asp_net_user_logins",
          column: "user_id");

      migrationBuilder.CreateIndex(
          name: "ix_asp_net_user_roles_role_id",
          table: "asp_net_user_roles",
          column: "role_id");

      migrationBuilder.CreateIndex(
          name: "email_index",
          table: "asp_net_users",
          column: "normalized_email");

      migrationBuilder.CreateIndex(
          name: "user_name_index",
          table: "asp_net_users",
          column: "normalized_user_name",
          unique: true);

      migrationBuilder.CreateIndex(
          name: "ix_categories_parent_category_id",
          table: "categories",
          column: "parent_category_id");

      migrationBuilder.CreateIndex(
          name: "ix_contacts_business_id",
          table: "contacts",
          column: "business_id");

      migrationBuilder.CreateIndex(
          name: "ix_expenses_application_user_id",
          table: "expenses",
          column: "application_user_id");

      migrationBuilder.CreateIndex(
          name: "ix_products_brand_id",
          table: "products",
          column: "brand_id");

      migrationBuilder.CreateIndex(
          name: "ix_products_category_id",
          table: "products",
          column: "category_id");

      migrationBuilder.CreateIndex(
          name: "ix_products_quality_id",
          table: "products",
          column: "quality_id");

      migrationBuilder.CreateIndex(
          name: "ix_purchase_order_invoice_line_items_purchase_order_invoice_id",
          table: "purchase_order_invoice_line_items",
          column: "purchase_order_invoice_id");

      migrationBuilder.CreateIndex(
          name: "ix_purchase_order_invoice_payments_purchase_order_invoice_id",
          table: "purchase_order_invoice_payments",
          column: "purchase_order_invoice_id");

      migrationBuilder.CreateIndex(
          name: "ix_purchase_order_invoices_purchase_order_id",
          table: "purchase_order_invoices",
          column: "purchase_order_id",
          unique: true);

      migrationBuilder.CreateIndex(
          name: "ix_purchase_order_line_items_product_id",
          table: "purchase_order_line_items",
          column: "product_id");

      migrationBuilder.CreateIndex(
          name: "ix_purchase_order_line_items_purchase_order_id",
          table: "purchase_order_line_items",
          column: "purchase_order_id");

      migrationBuilder.CreateIndex(
          name: "ix_purchase_order_note_purchase_order_id",
          table: "purchase_order_note",
          column: "purchase_order_id");

      migrationBuilder.CreateIndex(
          name: "ix_purchase_orders_supplier_id",
          table: "purchase_orders",
          column: "supplier_id");

      migrationBuilder.CreateIndex(
          name: "ix_sale_order_invoice_line_items_sale_order_invoice_id",
          table: "sale_order_invoice_line_items",
          column: "sale_order_invoice_id");

      migrationBuilder.CreateIndex(
          name: "ix_sale_order_invoice_payments_sale_order_invoice_id",
          table: "sale_order_invoice_payments",
          column: "sale_order_invoice_id");

      migrationBuilder.CreateIndex(
          name: "ix_sale_order_invoices_sale_order_id",
          table: "sale_order_invoices",
          column: "sale_order_id",
          unique: true);

      migrationBuilder.CreateIndex(
          name: "ix_sale_order_line_items_product_id",
          table: "sale_order_line_items",
          column: "product_id");

      migrationBuilder.CreateIndex(
          name: "ix_sale_order_line_items_sale_order_id",
          table: "sale_order_line_items",
          column: "sale_order_id");

      migrationBuilder.CreateIndex(
          name: "ix_sale_order_note_purchase_order_id",
          table: "sale_order_note",
          column: "purchase_order_id");

      migrationBuilder.CreateIndex(
          name: "ix_sale_order_note_sale_order_id",
          table: "sale_order_note",
          column: "sale_order_id");

      migrationBuilder.CreateIndex(
          name: "ix_sale_order_returns_product_id",
          table: "sale_order_returns",
          column: "product_id");

      migrationBuilder.CreateIndex(
          name: "ix_sale_order_returns_sale_order_invoice_id",
          table: "sale_order_returns",
          column: "sale_order_invoice_id");

      migrationBuilder.CreateIndex(
          name: "ix_sale_orders_customer_id",
          table: "sale_orders",
          column: "customer_id");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "addresses");

      migrationBuilder.DropTable(
          name: "asp_net_role_claims");

      migrationBuilder.DropTable(
          name: "asp_net_user_claims");

      migrationBuilder.DropTable(
          name: "asp_net_user_logins");

      migrationBuilder.DropTable(
          name: "asp_net_user_roles");

      migrationBuilder.DropTable(
          name: "asp_net_user_tokens");

      migrationBuilder.DropTable(
          name: "contacts");

      migrationBuilder.DropTable(
          name: "expenses");

      migrationBuilder.DropTable(
          name: "partners");

      migrationBuilder.DropTable(
          name: "purchase_order_invoice_line_items");

      migrationBuilder.DropTable(
          name: "purchase_order_invoice_payments");

      migrationBuilder.DropTable(
          name: "purchase_order_line_items");

      migrationBuilder.DropTable(
          name: "purchase_order_note");

      migrationBuilder.DropTable(
          name: "sale_order_invoice_line_items");

      migrationBuilder.DropTable(
          name: "sale_order_invoice_payments");

      migrationBuilder.DropTable(
          name: "sale_order_line_items");

      migrationBuilder.DropTable(
          name: "sale_order_note");

      migrationBuilder.DropTable(
          name: "sale_order_returns");

      migrationBuilder.DropTable(
          name: "asp_net_roles");

      migrationBuilder.DropTable(
          name: "asp_net_users");

      migrationBuilder.DropTable(
          name: "purchase_order_invoices");

      migrationBuilder.DropTable(
          name: "products");

      migrationBuilder.DropTable(
          name: "sale_order_invoices");

      migrationBuilder.DropTable(
          name: "purchase_orders");

      migrationBuilder.DropTable(
          name: "brands");

      migrationBuilder.DropTable(
          name: "categories");

      migrationBuilder.DropTable(
          name: "qualities");

      migrationBuilder.DropTable(
          name: "sale_orders");

      migrationBuilder.DropTable(
          name: "businesses");
    }
  }
}

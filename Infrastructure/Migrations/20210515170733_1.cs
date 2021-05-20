using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Infrastructure.Migrations
{
    public partial class _1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "DefaultFacility",
                columns: table => new
                {
                    FacilityFID = table.Column<Guid>(nullable: false),
                    UserFID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefaultFacility", x => new { x.FacilityFID, x.UserFID });
                });

            migrationBuilder.CreateTable(
                name: "Errand",
                columns: table => new
                {
                    ErrandID = table.Column<Guid>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<Guid>(nullable: false),
                    FacilityFID = table.Column<Guid>(nullable: false),
                    EmployeeFID = table.Column<Guid>(nullable: false),
                    ProviderFID = table.Column<Guid>(nullable: false),
                    OriginalDueDate = table.Column<DateTime>(nullable: false),
                    DueDate = table.Column<DateTime>(nullable: false),
                    CompleteDate = table.Column<DateTime>(nullable: true),
                    Status = table.Column<byte>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Report = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Errand", x => x.ErrandID);
                });

            migrationBuilder.CreateTable(
                name: "Facility",
                columns: table => new
                {
                    FacilityID = table.Column<Guid>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<Guid>(nullable: false),
                    CompanyName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    SecurityCode = table.Column<string>(nullable: true),
                    ProviderFID = table.Column<Guid>(nullable: false),
                    ProviderId1 = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facility", x => x.FacilityID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FacilityId = table.Column<Guid>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    Available = table.Column<bool>(nullable: false),
                    ProviderName = table.Column<string>(nullable: true),
                    ProviderFID = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Facility_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facility",
                        principalColumn: "FacilityID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_AspNetUsers_ProviderFID",
                        column: x => x.ProviderFID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Perimeter",
                columns: table => new
                {
                    PerimeterID = table.Column<Guid>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<Guid>(nullable: false),
                    FacilityFID = table.Column<Guid>(nullable: false),
                    LeftLoc = table.Column<int>(nullable: false),
                    TopLoc = table.Column<int>(nullable: false),
                    PerimeterName = table.Column<string>(nullable: true),
                    Scale = table.Column<decimal>(nullable: false),
                    SchemeImagePath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perimeter", x => x.PerimeterID);
                    table.ForeignKey(
                        name: "FK_Perimeter_Facility_FacilityFID",
                        column: x => x.FacilityFID,
                        principalTable: "Facility",
                        principalColumn: "FacilityID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Supplement",
                columns: table => new
                {
                    SupplementID = table.Column<Guid>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<Guid>(nullable: false),
                    SupplementName = table.Column<string>(nullable: true),
                    ExpirationDate = table.Column<DateTime>(nullable: false),
                    CertificateFilePath = table.Column<string>(nullable: true),
                    ProviderFID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplement", x => x.SupplementID);
                    table.ForeignKey(
                        name: "FK_Supplement_AspNetUsers_ProviderFID",
                        column: x => x.ProviderFID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trap",
                columns: table => new
                {
                    TrapID = table.Column<Guid>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<Guid>(nullable: false),
                    TrapName = table.Column<string>(nullable: true),
                    Color = table.Column<string>(nullable: true),
                    ProviderFID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trap", x => x.TrapID);
                    table.ForeignKey(
                        name: "FK_Trap_AspNetUsers_ProviderFID",
                        column: x => x.ProviderFID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Field",
                columns: table => new
                {
                    FieldID = table.Column<Guid>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<Guid>(nullable: false),
                    TrapFID = table.Column<Guid>(nullable: false),
                    FieldName = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    FieldType = table.Column<int>(nullable: false),
                    OptionList = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Field", x => x.FieldID);
                    table.ForeignKey(
                        name: "FK_Field_Trap_TrapFID",
                        column: x => x.TrapFID,
                        principalTable: "Trap",
                        principalColumn: "TrapID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Point",
                columns: table => new
                {
                    PointID = table.Column<Guid>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<Guid>(nullable: false),
                    PerimeterFID = table.Column<Guid>(nullable: false),
                    TrapFID = table.Column<Guid>(nullable: false),
                    SupplementFID = table.Column<Guid>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    LeftLoc = table.Column<int>(nullable: false),
                    TopLoc = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Point", x => x.PointID);
                    table.ForeignKey(
                        name: "FK_Point_Perimeter_PerimeterFID",
                        column: x => x.PerimeterFID,
                        principalTable: "Perimeter",
                        principalColumn: "PerimeterID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Point_Supplement_SupplementFID",
                        column: x => x.SupplementFID,
                        principalTable: "Supplement",
                        principalColumn: "SupplementID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Point_Trap_TrapFID",
                        column: x => x.TrapFID,
                        principalTable: "Trap",
                        principalColumn: "TrapID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PointReview",
                columns: table => new
                {
                    PointReviewID = table.Column<Guid>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<Guid>(nullable: false),
                    ErrandFID = table.Column<Guid>(nullable: false),
                    PointFID = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Report = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointReview", x => x.PointReviewID);
                    table.ForeignKey(
                        name: "FK_PointReview_Errand_ErrandFID",
                        column: x => x.ErrandFID,
                        principalTable: "Errand",
                        principalColumn: "ErrandID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PointReview_Point_PointFID",
                        column: x => x.PointFID,
                        principalTable: "Point",
                        principalColumn: "PointID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PointReviewRecord",
                columns: table => new
                {
                    PointReviewRecordID = table.Column<Guid>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<Guid>(nullable: false),
                    PointReviewFID = table.Column<Guid>(nullable: false),
                    FieldFID = table.Column<Guid>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointReviewRecord", x => x.PointReviewRecordID);
                    table.ForeignKey(
                        name: "FK_PointReviewRecord_Field_FieldFID",
                        column: x => x.FieldFID,
                        principalTable: "Field",
                        principalColumn: "FieldID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PointReviewRecord_PointReview_PointReviewFID",
                        column: x => x.PointReviewFID,
                        principalTable: "PointReview",
                        principalColumn: "PointReviewID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FacilityId",
                table: "AspNetUsers",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProviderFID",
                table: "AspNetUsers",
                column: "ProviderFID");

            migrationBuilder.CreateIndex(
                name: "IX_DefaultFacility_UserFID",
                table: "DefaultFacility",
                column: "UserFID");

            migrationBuilder.CreateIndex(
                name: "IX_Errand_EmployeeFID",
                table: "Errand",
                column: "EmployeeFID");

            migrationBuilder.CreateIndex(
                name: "IX_Errand_FacilityFID",
                table: "Errand",
                column: "FacilityFID");

            migrationBuilder.CreateIndex(
                name: "IX_Errand_ProviderFID",
                table: "Errand",
                column: "ProviderFID");

            migrationBuilder.CreateIndex(
                name: "IX_Facility_ProviderId1",
                table: "Facility",
                column: "ProviderId1");

            migrationBuilder.CreateIndex(
                name: "IX_Field_TrapFID",
                table: "Field",
                column: "TrapFID");

            migrationBuilder.CreateIndex(
                name: "IX_Perimeter_FacilityFID",
                table: "Perimeter",
                column: "FacilityFID");

            migrationBuilder.CreateIndex(
                name: "IX_Point_PerimeterFID",
                table: "Point",
                column: "PerimeterFID");

            migrationBuilder.CreateIndex(
                name: "IX_Point_SupplementFID",
                table: "Point",
                column: "SupplementFID");

            migrationBuilder.CreateIndex(
                name: "IX_Point_TrapFID",
                table: "Point",
                column: "TrapFID");

            migrationBuilder.CreateIndex(
                name: "IX_PointReview_ErrandFID",
                table: "PointReview",
                column: "ErrandFID");

            migrationBuilder.CreateIndex(
                name: "IX_PointReview_PointFID",
                table: "PointReview",
                column: "PointFID");

            migrationBuilder.CreateIndex(
                name: "IX_PointReviewRecord_FieldFID",
                table: "PointReviewRecord",
                column: "FieldFID");

            migrationBuilder.CreateIndex(
                name: "IX_PointReviewRecord_PointReviewFID",
                table: "PointReviewRecord",
                column: "PointReviewFID");

            migrationBuilder.CreateIndex(
                name: "IX_Supplement_ProviderFID",
                table: "Supplement",
                column: "ProviderFID");

            migrationBuilder.CreateIndex(
                name: "IX_Trap_ProviderFID",
                table: "Trap",
                column: "ProviderFID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DefaultFacility_AspNetUsers_UserFID",
                table: "DefaultFacility",
                column: "UserFID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DefaultFacility_Facility_FacilityFID",
                table: "DefaultFacility",
                column: "FacilityFID",
                principalTable: "Facility",
                principalColumn: "FacilityID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Errand_AspNetUsers_EmployeeFID",
                table: "Errand",
                column: "EmployeeFID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Errand_AspNetUsers_ProviderFID",
                table: "Errand",
                column: "ProviderFID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Errand_Facility_FacilityFID",
                table: "Errand",
                column: "FacilityFID",
                principalTable: "Facility",
                principalColumn: "FacilityID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Facility_AspNetUsers_ProviderId1",
                table: "Facility",
                column: "ProviderId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facility_AspNetUsers_ProviderId1",
                table: "Facility");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "DefaultFacility");

            migrationBuilder.DropTable(
                name: "PointReviewRecord");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Field");

            migrationBuilder.DropTable(
                name: "PointReview");

            migrationBuilder.DropTable(
                name: "Errand");

            migrationBuilder.DropTable(
                name: "Point");

            migrationBuilder.DropTable(
                name: "Perimeter");

            migrationBuilder.DropTable(
                name: "Supplement");

            migrationBuilder.DropTable(
                name: "Trap");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Facility");
        }
    }
}

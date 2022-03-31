using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mre.Sb.Base.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AbpFeatureValues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ProviderKey = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpFeatureValues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpTenants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpTenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoriaAuditable",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriaAuditable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClaimTipo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Requerido = table.Column<bool>(type: "bit", nullable: false),
                    EsEstatico = table.Column<bool>(type: "bit", nullable: false),
                    ExpRegular = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    ExpRegularDesc = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ValorTipo = table.Column<int>(type: "int", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimTipo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Configuracion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Valor = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    Proveedor = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ProveedorClave = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuracion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogSeguridad",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Aplicacion = table.Column<string>(type: "nvarchar(96)", maxLength: 96, nullable: true),
                    Identidad = table.Column<string>(type: "nvarchar(96)", maxLength: 96, nullable: true),
                    Accion = table.Column<string>(type: "nvarchar(96)", maxLength: 96, nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Usuario = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TenantName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ClienteId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    CorrelacionId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ClienteIp = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    NavegadorInfo = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogSeguridad", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermisoOtorgado",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Proveedor = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ProveedorClave = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermisoOtorgado", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rol",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    NombreNormalizado = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    EsDefecto = table.Column<bool>(type: "bit", nullable: false),
                    EsEstitico = table.Column<bool>(type: "bit", nullable: false),
                    EsPublico = table.Column<bool>(type: "bit", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rol", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UniOrganizacional",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Codigo = table.Column<string>(type: "nvarchar(95)", maxLength: 95, nullable: false),
                    NombreVisualizar = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UniOrganizacional", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UniOrganizacional_UniOrganizacional_ParentId",
                        column: x => x.ParentId,
                        principalTable: "UniOrganizacional",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Usuario = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    UsuarioNormalizado = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Apellido = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    EmailNormalizado = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    EmailConfirmado = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ClaveHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    AutExterna = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Telefono = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    EsActivo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    TelefonoConfirmado = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DobleFactorActivo = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    BloqueoFin = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    BloqueoActivo = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TotalAccesoFallidos = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    CambiarClaveSigAcceso = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioEnlace",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioOrigenId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SourceTenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UsuarioDestinoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TargetTenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioEnlace", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpTenantConnectionStrings",
                columns: table => new
                {
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpTenantConnectionStrings", x => new { x.TenantId, x.Name });
                    table.ForeignKey(
                        name: "FK_AbpTenantConnectionStrings_AbpTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "AbpTenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Auditable",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoriaId = table.Column<string>(type: "nvarchar(6)", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Item = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auditable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auditable_CategoriaAuditable_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "CategoriaAuditable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolClaim",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ClaimTipo = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ClaimValor = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolClaim_Rol_RolId",
                        column: x => x.RolId,
                        principalTable: "Rol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UniOrganizacionalRol",
                columns: table => new
                {
                    RolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UniOrgId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UniOrganizacionalRol", x => new { x.UniOrgId, x.RolId });
                    table.ForeignKey(
                        name: "FK_UniOrganizacionalRol_Rol_RolId",
                        column: x => x.RolId,
                        principalTable: "Rol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UniOrganizacionalRol_UniOrganizacional_UniOrgId",
                        column: x => x.UniOrgId,
                        principalTable: "UniOrganizacional",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UniOrganizacionalUsuario",
                columns: table => new
                {
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UniOrgId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UniOrganizacionalUsuario", x => new { x.UniOrgId, x.UsuarioId });
                    table.ForeignKey(
                        name: "FK_UniOrganizacionalUsuario_UniOrganizacional_UniOrgId",
                        column: x => x.UniOrgId,
                        principalTable: "UniOrganizacional",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UniOrganizacionalUsuario_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioClaim",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ClaimTipo = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ClaimValor = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuarioClaim_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioHistorico",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaveHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioHistorico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuarioHistorico_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioLogin",
                columns: table => new
                {
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProveedor = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProveedorClave = table.Column<string>(type: "nvarchar(196)", maxLength: 196, nullable: false),
                    ProveedorNombre = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioLogin", x => new { x.UsuarioId, x.LoginProveedor });
                    table.ForeignKey(
                        name: "FK_UsuarioLogin_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioRol",
                columns: table => new
                {
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioRol", x => new { x.UsuarioId, x.RolId });
                    table.ForeignKey(
                        name: "FK_UsuarioRol_Rol_RolId",
                        column: x => x.RolId,
                        principalTable: "Rol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioRol_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioToken",
                columns: table => new
                {
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProveedor = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioToken", x => new { x.UsuarioId, x.LoginProveedor, x.Nombre });
                    table.ForeignKey(
                        name: "FK_UsuarioToken_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Auditar",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuditableId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Acciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auditar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auditar_Auditable_AuditableId",
                        column: x => x.AuditableId,
                        principalTable: "Auditable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbpFeatureValues_Name_ProviderName_ProviderKey",
                table: "AbpFeatureValues",
                columns: new[] { "Name", "ProviderName", "ProviderKey" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpTenants_Name",
                table: "AbpTenants",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Auditable_CategoriaId_Item",
                table: "Auditable",
                columns: new[] { "CategoriaId", "Item" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Auditar_AuditableId",
                table: "Auditar",
                column: "AuditableId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuracion_Nombre_Proveedor_ProveedorClave",
                table: "Configuracion",
                columns: new[] { "Nombre", "Proveedor", "ProveedorClave" },
                unique: true,
                filter: "[Proveedor] IS NOT NULL AND [ProveedorClave] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_LogSeguridad_TenantId_Accion",
                table: "LogSeguridad",
                columns: new[] { "TenantId", "Accion" });

            migrationBuilder.CreateIndex(
                name: "IX_LogSeguridad_TenantId_Aplicacion",
                table: "LogSeguridad",
                columns: new[] { "TenantId", "Aplicacion" });

            migrationBuilder.CreateIndex(
                name: "IX_LogSeguridad_TenantId_Identidad",
                table: "LogSeguridad",
                columns: new[] { "TenantId", "Identidad" });

            migrationBuilder.CreateIndex(
                name: "IX_LogSeguridad_TenantId_UserId",
                table: "LogSeguridad",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_PermisoOtorgado_TenantId_Nombre_Proveedor_ProveedorClave",
                table: "PermisoOtorgado",
                columns: new[] { "TenantId", "Nombre", "Proveedor", "ProveedorClave" },
                unique: true,
                filter: "[TenantId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Rol_NombreNormalizado",
                table: "Rol",
                column: "NombreNormalizado");

            migrationBuilder.CreateIndex(
                name: "IX_RolClaim_RolId",
                table: "RolClaim",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_UniOrganizacional_Codigo",
                table: "UniOrganizacional",
                column: "Codigo");

            migrationBuilder.CreateIndex(
                name: "IX_UniOrganizacional_ParentId",
                table: "UniOrganizacional",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_UniOrganizacionalRol_RolId_UniOrgId",
                table: "UniOrganizacionalRol",
                columns: new[] { "RolId", "UniOrgId" });

            migrationBuilder.CreateIndex(
                name: "IX_UniOrganizacionalUsuario_UsuarioId_UniOrgId",
                table: "UniOrganizacionalUsuario",
                columns: new[] { "UsuarioId", "UniOrgId" });

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Email",
                table: "Usuario",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_EmailNormalizado",
                table: "Usuario",
                column: "EmailNormalizado");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Usuario",
                table: "Usuario",
                column: "Usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_UsuarioNormalizado",
                table: "Usuario",
                column: "UsuarioNormalizado");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioClaim_UsuarioId",
                table: "UsuarioClaim",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioEnlace_UsuarioOrigenId_SourceTenantId_UsuarioDestinoId_TargetTenantId",
                table: "UsuarioEnlace",
                columns: new[] { "UsuarioOrigenId", "SourceTenantId", "UsuarioDestinoId", "TargetTenantId" },
                unique: true,
                filter: "[SourceTenantId] IS NOT NULL AND [TargetTenantId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioHistorico_UsuarioId",
                table: "UsuarioHistorico",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioLogin_LoginProveedor_ProveedorClave",
                table: "UsuarioLogin",
                columns: new[] { "LoginProveedor", "ProveedorClave" });

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioRol_RolId_UsuarioId",
                table: "UsuarioRol",
                columns: new[] { "RolId", "UsuarioId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbpFeatureValues");

            migrationBuilder.DropTable(
                name: "AbpTenantConnectionStrings");

            migrationBuilder.DropTable(
                name: "Auditar");

            migrationBuilder.DropTable(
                name: "ClaimTipo");

            migrationBuilder.DropTable(
                name: "Configuracion");

            migrationBuilder.DropTable(
                name: "LogSeguridad");

            migrationBuilder.DropTable(
                name: "PermisoOtorgado");

            migrationBuilder.DropTable(
                name: "RolClaim");

            migrationBuilder.DropTable(
                name: "UniOrganizacionalRol");

            migrationBuilder.DropTable(
                name: "UniOrganizacionalUsuario");

            migrationBuilder.DropTable(
                name: "UsuarioClaim");

            migrationBuilder.DropTable(
                name: "UsuarioEnlace");

            migrationBuilder.DropTable(
                name: "UsuarioHistorico");

            migrationBuilder.DropTable(
                name: "UsuarioLogin");

            migrationBuilder.DropTable(
                name: "UsuarioRol");

            migrationBuilder.DropTable(
                name: "UsuarioToken");

            migrationBuilder.DropTable(
                name: "AbpTenants");

            migrationBuilder.DropTable(
                name: "Auditable");

            migrationBuilder.DropTable(
                name: "UniOrganizacional");

            migrationBuilder.DropTable(
                name: "Rol");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "CategoriaAuditable");
        }
    }
}

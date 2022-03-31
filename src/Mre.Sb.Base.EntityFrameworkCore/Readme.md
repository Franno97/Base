
#Principal
Add-Migration "Inicial"  -Context BaseDbContext

Update-Database -Context BaseDbContext

# Generar script desde la primera migracion hasta la ultima
Script-Migration -Context BaseDbContext 0


#IdentityServer
Add-Migration "Inicial" -OutputDir "IdentityServerDbMigrations" -Context IdentityServerDbContext
Update-Database -Context IdentityServerDbContext

# Generar script desde la primera migracion hasta la ultima
Script-Migration -Context IdentityServerDbContext 0
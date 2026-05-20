USE [ChatAppDB];
GO

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF OBJECT_ID(N'dbo.users', N'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.users;
END
GO

IF OBJECT_ID(N'dbo.roles', N'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.roles;
END
GO

CREATE TABLE dbo.roles
(
    id INT IDENTITY(1,1) NOT NULL,
    name NVARCHAR(50) NOT NULL,
    created_at DATETIME2(0) NOT NULL CONSTRAINT DF_roles_created_at DEFAULT SYSUTCDATETIME(),
    CONSTRAINT PK_roles PRIMARY KEY CLUSTERED (id),
    CONSTRAINT UQ_roles_name UNIQUE (name)
);
GO

CREATE TABLE dbo.users
(
    id UNIQUEIDENTIFIER NOT NULL CONSTRAINT DF_users_id DEFAULT NEWID(),
    full_name NVARCHAR(255) NOT NULL,
    email NVARCHAR(255) NOT NULL,
    password_hash NVARCHAR(512) NOT NULL,
    role_id INT NOT NULL,
    is_active BIT NOT NULL CONSTRAINT DF_users_is_active DEFAULT 1,
    created_at DATETIME2(0) NOT NULL CONSTRAINT DF_users_created_at DEFAULT SYSUTCDATETIME(),
    updated_at DATETIME2(0) NOT NULL CONSTRAINT DF_users_updated_at DEFAULT SYSUTCDATETIME(),
    CONSTRAINT PK_users PRIMARY KEY CLUSTERED (id),
    CONSTRAINT FK_users_roles_role_id FOREIGN KEY (role_id) REFERENCES dbo.roles(id),
    CONSTRAINT UQ_users_email UNIQUE (email)
);
GO

CREATE INDEX IX_users_role_id ON dbo.users(role_id);
GO

INSERT INTO dbo.roles(name)
VALUES (N'admin'), (N'user');
GO

CREATE OR ALTER TRIGGER dbo.trg_users_set_updated_at
ON dbo.users
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE u
    SET updated_at = SYSUTCDATETIME()
    FROM dbo.users u
    INNER JOIN inserted i ON i.id = u.id;
END;
GO

ALTER TABLE tblEmpleado ADD Vigente BIT NULL
GO

UPDATE tblEmpleado SET Vigente = 1
GO

ALTER TABLE tblEmpleado ALTER COLUMN Vigente BIT NOT NULL
GO
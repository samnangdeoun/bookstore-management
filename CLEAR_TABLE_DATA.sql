USE BookStoreDB;
GO

BEGIN TRY
    BEGIN TRANSACTION;

    -- Disable all constraints
    EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';

    -- Delete data from all tables
    EXEC sp_MSforeachtable 'DELETE FROM ?';

    -- Re-enable all constraints
    EXEC sp_MSforeachtable 'ALTER TABLE ? CHECK CONSTRAINT ALL';

    COMMIT TRANSACTION;
    PRINT 'All data has been successfully cleared from the database.';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;
    
    PRINT 'An error occurred while clearing the database:';
    PRINT ERROR_MESSAGE();
END CATCH
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_ACT_Find_Batch
GO

CREATE PROC spu_ACT_Find_Batch
    @StartDate DATETIME = NULL,  
    @EndDate DATETIME = NULL,
    @BatchID INT = NULL,
    @BatchType INT = NULL,
    @PaymentMethod INT = NULL,
    @BatchStatus INT = NULL,
    @Mode VARCHAR(20) = ''

AS 
BEGIN

    IF UPPER(@Mode) != 'DELETE' 
    BEGIN
        SELECT
            Batch_id,
            CONVERT(VARCHAR(10),Created_Date,103) AS Created_Date,
            bt.Description AS BatchType,
            m.Description AS PaymentMethod,
            'Bank Code' AS BankCode,
            total_amount,
            total_transactions,
            bs.Description AS Status
        FROM 
            Batch b
            LEFT OUTER JOIN Batch_Type bt
                ON b.Batch_Type_id = bt.Batch_Type_id
            LEFT OUTER JOIN MediaType_Validation m
                ON b.mediatype_validation_id = m.mediatype_validation_id
            LEFT OUTER JOIN batchStatus bs
                ON b.batchstatus_id = bs.batchstatus_id
        WHERE
            (
             CONVERT(VARCHAR(10),b.created_date,112) >= CONVERT(VARCHAR(10),@StartDate,112) 
             OR @StartDate IS NULL
            )
            AND (
                 CONVERT(VARCHAR(10),b.created_date,112) >= CONVERT(VARCHAR(10),@EndDate,112)
                 OR @EndDate IS NULL
                )
            AND (
                 b.Batch_ID = @BatchID 
                 OR @BatchID IS NULL
                )
            AND (
                 b.Batch_Type_ID = @BatchType 
                 OR @BatchType IS NULL
                )
            AND (
                 b.mediatype_validation_id = @PaymentMethod 
                 OR @PaymentMethod IS NULL
                )
            AND (
                 b.batchstatus_id = @BatchStatus
                 OR @BatchStatus IS NULL
                )
    END
    ELSE
    BEGIN
        SELECT
            Batch_id,
            CONVERT(VARCHAR(10),Created_Date,103) AS Created_Date,
            bt.Description AS BatchType,
            m.Description AS PaymentMethod,
            'Bank Code' AS BankCode,
            total_amount,
            total_transactions,
            bs.Description AS Status

        FROM Batch b
            LEFT OUTER JOIN Batch_Type bt
                ON b.Batch_Type_id = bt.Batch_Type_id
            LEFT OUTER JOIN MediaType_Validation m
                ON b.mediatype_validation_id = m.mediatype_validation_id
            INNER JOIN batchStatus bs
                ON b.batchstatus_id = bs.batchstatus_id
        WHERE
            (
             CONVERT(VARCHAR(10),b.created_date,112) >= CONVERT(VARCHAR(10),@StartDate,112) 
             OR @StartDate IS NULL
            )
            AND (
                 CONVERT(VARCHAR(10),b.created_date,112) >= CONVERT(VARCHAR(10),@EndDate,112) 
                 OR @EndDate IS NULL
                )
            AND (
                 b.Batch_ID = @BatchID 
                 OR @BatchID IS NULL
                )
            AND (
                 b.Batch_Type_ID = @BatchType 
                 OR @BatchType IS NULL
                )
            AND (
                 b.mediatype_validation_id = @PaymentMethod 
                 OR @PaymentMethod IS NULL
                )
            AND (
                 bs.code = 'R' 
                 OR bs.Code = 'A'
                )
    END
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


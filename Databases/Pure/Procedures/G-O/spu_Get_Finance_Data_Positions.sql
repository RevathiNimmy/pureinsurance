SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_Finance_Data_Positions'
GO

CREATE PROCEDURE spu_Get_Finance_Data_Positions
AS

SELECT  
    ( 
        SELECT 
            output_order 
        FROM pfedidefinitionfields 
        WHERE pfedidefinition_id = d.pfedidefinition_id
        AND column_name = 'AutoGenPlanRef'
        AND section = 'B'
    ),
    (
        SELECT 
            MAX(output_order)
        FROM pfedidefinitionfields 
        WHERE pfedidefinition_id = d.pfedidefinition_id
        AND section = 'B'
    )
    +
    ( 
        SELECT 
            output_order 
        FROM pfedidefinitionfields 
        WHERE pfedidefinition_id = d.pfedidefinition_id
        AND column_name = 'TransactionType' 
        AND section = 'T'
    ),        
    ( 
        SELECT 
            output_order 
        FROM pfedidefinitionfields 
        WHERE pfedidefinition_id = d.pfedidefinition_id
        AND column_name = 'ClientAddr1' 
        AND section = 'B'
    ),
    ( 
        SELECT 
            output_order 
        FROM pfedidefinitionfields 
        WHERE pfedidefinition_id = d.pfedidefinition_id
        AND column_name = 'ClientAddr2' 
        AND section = 'B'
    ),
    ( 
        SELECT 
            output_order 
        FROM pfedidefinitionfields 
        WHERE pfedidefinition_id = d.pfedidefinition_id
        AND column_name = 'ClietnAddr3' 
        AND section = 'B'
    ),
    ( 
        SELECT 
            output_order 
        FROM pfedidefinitionfields 
        WHERE pfedidefinition_id = d.pfedidefinition_id
        AND column_name = 'ClientAddr4' 
        AND section = 'B'
    ),
    ( 
        SELECT 
            output_order 
        FROM pfedidefinitionfields 
        WHERE pfedidefinition_id = d.pfedidefinition_id
        AND column_name = 'ClientPcode' 
        AND section = 'B'
    ),
    ( 
        SELECT 
            output_order 
        FROM pfedidefinitionfields 
        WHERE pfedidefinition_id = d.pfedidefinition_id
        AND column_name = 'TotalGrossPremium' 
        AND section = 'B'
    ),
    ( 
        SELECT 
            output_order 
        FROM pfedidefinitionfields 
        WHERE pfedidefinition_id = d.pfedidefinition_id
        AND column_name = 'ValueOfInterest' 
        AND section = 'B'
    )
FROM PFEDIDefinition d
WHERE code = 'PC'

GO

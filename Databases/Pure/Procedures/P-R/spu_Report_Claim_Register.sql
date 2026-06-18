SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Report_Claim_Register'
GO


CREATE PROCEDURE spu_Report_Claim_Register
AS

------------------------------------------------
-- Created by Jude Killip
-- 11/08/2000
-- RSA Reports - ClaimRegister.rpt
--  *Dummy data for work in progress
------------------------------------------------
CREATE TABLE #tempRSAClaimReg
(
    LastDayReport datetime NULL,
    LastWeekReport datetime NULL,
    LastMonthReport datetime NULL,
    RiskGroupCode varchar (10) NULL,
    RiskGroupCodeDesc varchar (255) NULL,
    ProductCode varchar (10) NULL,
    ProductDesc varchar (100) NULL,
    TransactionCode varchar (10) NULL,
    TransactionCodeDesc varchar (100) NULL,
    UWYearOfClaim datetime NULL,
    TransDate datetime NULL,
    TransRef varchar (30) NULL,
    Agency varchar (100) NULL,
    FromDate datetime NULL,
    ToDate datetime NULL,
    SumInsured decimal (19,4) NULL,
    FacReins decimal (19,4) NULL,
    PolNum varchar (30) NULL,
    Client varchar (100) NULL,
    ClaimValue decimal (19,4) NULL,
    ClaimPaidGross decimal (19,4) NULL,
    ClaimOutstandingGross decimal (19,4) NULL,
    GrossPremium decimal (19,4) NULL
)
INSERT INTO #tempRSAClaimReg
    SELECT Convert(Datetime, '08/07/2000'), Convert(Datetime, '08/03/2000'), Convert(Datetime, '07/31/2000'), 'PRIVMED', 'Private Medical', 'PC1', 'Medical Product 1', 'TRANTYPE1', 'New Business', Convert(Datetime, '01/07/2000'), Convert(Datetime, '08/09/2000'), 'TRANS1', 'Agent1', Convert(Datetime, '07/20/2000'), Convert(Datetime, '07/20/2001'), 19300, 0, 'POL001', 'Client1', 19300, 9650, 9650, 42.88889

INSERT INTO #tempRSAClaimReg
    SELECT Convert(Datetime, '08/07/2000'), Convert(Datetime, '08/03/2000'), Convert(Datetime, '07/31/2000'), 'REINS', 'Reinsurance', 'PC2', 'Reinsurance Product 2', 'TRANTYPE2', 'Renewal', Convert(Datetime, '01/03/2000'), Convert(Datetime, '08/05/2000'), 'TRANS2', 'Agent1', Convert(Datetime, '07/16/2000'), Convert(Datetime, '07/16/2001'), 28950, 0, 'POL002', 'Client1', 28950, 8685, 20265, 64.33333

INSERT INTO #tempRSAClaimReg
    SELECT Convert(Datetime, '08/07/2000'), Convert(Datetime, '08/03/2000'), Convert(Datetime, '07/31/2000'), 'MISC',
    'Miscellaneous', 'PC3', 'Miscellaneous Product 3', 'TRANTYPE1', 'New Business', Convert(Datetime, '12/18/1999'),
    Convert(Datetime, '07/20/2000'), 'TRANS3', 'Agent2', Convert(Datetime, '06/30/2000'), Convert(Datetime, '06/30/2001'),
    119135, 0, 'POL003', 'LastMonth', 119135, 119135, 0, 264.7445

INSERT INTO #tempRSAClaimReg
    SELECT Convert(Datetime, '08/07/2000'), Convert(Datetime, '08/03/2000'), Convert(Datetime, '07/31/2000'),
    'PRIVMED', 'Private Medical', 'PC1', 'Medical Product 1', 'TRANTYPE1', 'New Business', Convert(Datetime, '12/30/1999'),
    Convert(Datetime, '08/01/2000'), 'TRANS4', 'Agent1', Convert(Datetime, '07/12/2000'), Convert(Datetime, '07/12/2001'),
    48250, 0, 'POL004', 'Client1', 48250, 36187.5, 12062.5, 107.2222

INSERT INTO #tempRSAClaimReg
    SELECT Convert(Datetime, '08/07/2000'), Convert(Datetime, '08/03/2000'), Convert(Datetime, '07/31/2000'), 'PRIVMED',
    'Private Medical', 'PC1', 'Medical Product 1', 'TRANTYPE1', 'New Business', Convert(Datetime, '01/07/2000'),
    Convert(Datetime, '08/09/2000'), 'TRANS5', 'Agent1', Convert(Datetime, '07/20/2000'), Convert(Datetime, '07/20/2001'),
    62725, 0, 'POL005', 'Client1', 62725, 21953.75, 40771.25, 139.3889

INSERT INTO #tempRSAClaimReg
    SELECT Convert(Datetime, '08/07/2000'), Convert(Datetime, '08/03/2000'), Convert(Datetime, '07/31/2000'), 'MISC',
    'Miscellaneous', 'PC6', 'Miscellaneous Product 6', 'TRANTYPE1', 'New Business', Convert(Datetime, '01/07/2000'),
    Convert(Datetime, '08/09/2000'), 'TRANS6', 'Agent1', Convert(Datetime, '07/20/2000'), Convert(Datetime, '07/20/2001'),
    96500, 0, 'POL006', 'Client1', 96500, 48250, 48250, 214.4444

INSERT INTO #tempRSAClaimReg
    SELECT Convert(Datetime, '08/07/2000'), Convert(Datetime, '08/03/2000'), Convert(Datetime, '07/31/2000'), 'MISC',
    'Miscellaneous', 'PC7', 'Miscellaneous Product 7', 'TRANTYPE1', 'New Business', Convert(Datetime, '01/03/2000'),
    Convert(Datetime, '08/05/2000'), 'TRANS7', 'Reins1', Convert(Datetime, '07/16/2000'), Convert(Datetime, '07/16/2001'),
    632062.5, 0, 'POL007', 'Client1', 632062.5, 189618.7, 442443.7, 1404.583

INSERT INTO #tempRSAClaimReg
    SELECT Convert(Datetime, '08/07/2000'), Convert(Datetime, '08/03/2000'), Convert(Datetime, '07/31/2000'), 'MISC',
    'Miscellaneous', 'PC3', 'Miscellaneous Product 3', 'TRANTYPE2', 'Renewal', Convert(Datetime, '01/03/2000'),
    Convert(Datetime, '08/05/2000'), 'TRANS8', 'Reins2', Convert(Datetime, '07/16/2000'), Convert(Datetime, '07/16/2001'),
    31039.23, 400, 'POL008', 'Client3', 31039.23, 31039.23, 0, 68.97606

INSERT INTO #tempRSAClaimReg
    SELECT Convert(Datetime, '08/07/2000'), Convert(Datetime, '08/03/2000'), Convert(Datetime, '07/31/2000'), 'MISC',
    'Miscellaneous', 'PC9', 'Miscellaneous Product 9', 'TRANTYPE1', 'New Business', Convert(Datetime, '12/31/1999'),
    Convert(Datetime, '08/02/2000'), 'TRANS9', 'Reins1', Convert(Datetime, '07/13/2000'), Convert(Datetime, '07/13/2001'),
    945504.1, 0, 'POL009', 'Client3', 945504.1, 709128.1, 236376, 2101.12

INSERT INTO #tempRSAClaimReg
    SELECT Convert(Datetime, '08/07/2000'), Convert(Datetime, '08/03/2000'), Convert(Datetime, '07/31/2000'), 'MISC',
    'Miscellaneous', 'PC10', 'Miscellaneous Product 10', 'TRANTYPE1', 'New Business', Convert(Datetime, '01/07/2000'),
    Convert(Datetime, '08/09/2000'), 'TRANS10', 'Reins1', Convert(Datetime, '07/20/2000'), Convert(Datetime, '07/20/2001'),
    623837.8, 0, 'POL010', 'Client1', 623837.8, 218343.2, 405494.5, 1386.306

INSERT INTO #tempRSAClaimReg
    SELECT Convert(Datetime, '08/07/2000'), Convert(Datetime, '08/03/2000'), Convert(Datetime, '07/31/2000'), 'PRIVMED',

    'Private Medical', 'PC11', 'Medical Product 11', 'TRANTYPE1', 'New Business', Convert(Datetime, '10/08/1999'),
    Convert(Datetime, '05/10/2000'), 'TRANS11', 'Reins1', Convert(Datetime, '04/20/2000'), Convert(Datetime, '04/20/2001'), 438635.9, 0, 'POL011', 'Client1', 438635.9, 210106.6, 228529.3, 974.7465

INSERT INTO #tempRSAClaimReg
    SELECT Convert(Datetime, '08/07/2000'), Convert(Datetime, '08/03/2000'), Convert(Datetime, '07/31/2000'), 'REINS',
    'Reinsurance', 'PC2', 'Reinsurance Product 2', 'TRANTYPE1', 'New Business', Convert(Datetime, '01/01/2000'),
    Convert(Datetime, '08/03/2000'), 'TRANS12', 'Reins1', Convert(Datetime, '07/14/2000'), Convert(Datetime, '07/14/2001'),
    9313421, 0, 'POL012', 'Client1', 9313421, 6426260, 2887160, 20696.49

INSERT INTO #tempRSAClaimReg
    SELECT Convert(Datetime, '08/07/2000'), Convert(Datetime, '08/03/2000'), Convert(Datetime, '07/31/2000'), 'REINS',
    'Reinsurance', 'PC2', 'Reinsurance Product 2', 'TRANTYPE2', 'Renewal', Convert(Datetime, '01/03/2000'),
    Convert(Datetime, '08/05/2000'), 'TRANS13', 'Agent1', Convert(Datetime, '07/16/2000'), Convert(Datetime, '07/16/2001'),
    18520202, 0, 'POL013', 'Client4', 18520202, 11501045, 7019156, 41156

INSERT INTO #tempRSAClaimReg
    SELECT Convert(Datetime, '08/07/2000'), Convert(Datetime, '08/03/2000'), Convert(Datetime, '07/31/2000'), 'REINS',
    'Reinsurance', 'PC22', 'Reinsurance Product 22', 'TRANTYPE2', 'Renewal', Convert(Datetime, '01/07/2000'),
    Convert(Datetime, '08/09/2000'), 'TRANS14', 'Agent1', Convert(Datetime, '07/20/2000'), Convert(Datetime, '07/20/2001'),
    2046970, 0, 'POL014', 'Client1', 2046970, 2046970, 0, 4548.821

INSERT INTO #tempRSAClaimReg
    SELECT Convert(Datetime, '08/07/2000'), Convert(Datetime, '08/03/2000'), Convert(Datetime, '07/31/2000'), 'MISC',
    'Miscellaneous', 'PC3', 'Miscellaneous Product 3', 'TRANTYPE1', 'New Business', Convert(Datetime, '01/04/2000'),
    Convert(Datetime, '08/06/2000'), 'TRANS15', 'Agent1', Convert(Datetime, '07/17/2000'), Convert(Datetime, '07/17/2001'),
    57900, 300, 'POL015', 'Client4', 57900, 43425, 14475, 128.6667

INSERT INTO #tempRSAClaimReg
    SELECT Convert(Datetime, '08/07/2000'), Convert(Datetime, '08/03/2000'), Convert(Datetime, '07/31/2000'), 'PRIVMED',
    'Private Medical', 'PC11', 'Medical Product 11', 'TRANTYPE1', 'New Business', Convert(Datetime, '10/03/1999'),
    Convert(Datetime, '05/05/2000'), 'TRANS16', 'Agent2', Convert(Datetime, '04/15/2000'), Convert(Datetime, '04/15/2001'),
    945702.9, 0, 'POL016', 'Client1', 945702.9, 330996, 614706.9, 2101.562

INSERT INTO #tempRSAClaimReg
    SELECT Convert(Datetime, '08/07/2000'), Convert(Datetime, '08/03/2000'), Convert(Datetime, '07/31/2000'), 'PRIVMED',
    'Private Medical', 'PC1', 'Medical Product 1', 'TRANTYPE2', 'Renewal', Convert(Datetime, '01/03/2000'),
    Convert(Datetime, '08/05/2000'), 'TRANS17', 'Agent1', Convert(Datetime, '07/16/2000'), Convert(Datetime, '07/16/2001'),
    117537, 0, 'POL017', 'Client5', 117537, 56300.22, 61236.78, 261.1933

INSERT INTO #tempRSAClaimReg
    SELECT Convert(Datetime, '08/07/2000'), Convert(Datetime, '08/03/2000'), Convert(Datetime, '07/31/2000'), 'REINS',
    'Reinsurance', 'PC2', 'Reinsurance Product 2', 'TRANTYPE1', 'New Business', Convert(Datetime, '12/05/1999'),
    Convert(Datetime, '07/07/2000'), 'TRANS18', 'Agent2', Convert(Datetime, '06/17/2000'), Convert(Datetime, '06/17/2001'),
    92948.8, 0, 'POL018', 'Client1', 92948.8, 64134.67, 28814.13, 206.5529

INSERT INTO #tempRSAClaimReg
    SELECT Convert(Datetime, '08/07/2000'), Convert(Datetime, '08/03/2000'), Convert(Datetime, '07/31/2000'), 'REINS',
    'Reinsurance', 'PC2', 'Reinsurance Product 2', 'TRANTYPE1', 'New Business', Convert(Datetime, '01/07/2000'),
    Convert(Datetime, '08/09/2000'), 'TRANS19', 'Agent1', Convert(Datetime, '07/20/2000'), Convert(Datetime, '07/20/2001'),
    31459, 0, 'POL019', 'Client1', 31459, 19536.04, 11922.96, 69.90889

INSERT INTO #tempRSAClaimReg
    SELECT Convert(Datetime, '08/07/2000'), Convert(Datetime, '08/03/2000'), Convert(Datetime, '07/31/2000'), 'MISC',
    'Miscellaneous', 'PC20', 'Miscellaneous Product 20', 'TRANTYPE1', 'New Business', Convert(Datetime, '12/30/1999'),
    Convert(Datetime, '08/01/2000'), 'TRANS20', 'Agent1', Convert(Datetime, '07/12/2000'), Convert(Datetime, '07/12/2001'),
    92515.52, 0, 'POL020', 'Client2', 92515.52, 44314.93, 48200.58, 205.59

Select * FROM #tempRSAClaimReg
DROP TABLE #tempRSAClaimReg
GO



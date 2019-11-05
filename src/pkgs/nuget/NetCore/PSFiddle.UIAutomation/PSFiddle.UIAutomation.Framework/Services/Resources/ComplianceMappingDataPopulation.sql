DECLARE @FileName NVARCHAR(200)
DECLARE @OrganizationKey BIGINT
DECLARE @UserEmail NVARCHAR(200)
DECLARE @UserKey BIGINT
DECLARE @AuditDetailsTypeKey INT
DECLARE @Purchasing BIT
DECLARE @Linking BIT

SET @UserKey         = '$(UserKey)'
SET @FileName        = '$(FileName)'
SET @UserEmail       = '$(UserEmail)'
SET @OrganizationKey = '$(OrganizationKey)'
SET @Purchasing      = '$(Purchasing)'
SET @Linking         = '$(Linking)'

-- Parameter Values -- 
SELECT @AuditDetailsTypeKey        = ParameterKey FROM Parameter WHERE ParameterValue    = 'TradeDirectory'

DECLARE @CorrelationID NVARCHAR(200)
-- Get Correlation ID
SELECT @CorrelationID = AuditText1 FROM dbo.AuditHistory WHERE AuditDetails = @FileName AND AuditDetailsTypeKey = @AuditDetailsTypeKey
IF @CorrelationID IS NULL THROW 5100, 'File Not found', 1

DECLARE @SourceCompanyDetails AS TABLE ( 
	ID	INT IDENTITY(1,1)
	, SourceIdentifier	NVARCHAR(128)
	, SourceIdentifierID1	NVARCHAR(128)
	, SourceIdentifierID2	NVARCHAR(128)
	, SourceIdentifierKey	BIGINT
	, OrganizationKey	BIGINT
	, StagingOrganizationKey	BIGINT
	, MatchScore decimal(18,2)
	, CreditRating decimal(18,2)
)

DECLARE @ComplianceCompanyDetails AS TABLE (
	ID	INT IDENTITY(1,1)
	, ComplianceType		NVARCHAR(128)
	, ComplianceUniqueId	NVARCHAR(128)
	, [DateID]			    NVARCHAR(128)
	, [BusinessName]		NVARCHAR(128)
	, [Description]			NVARCHAR(128)
	, [Telephone]			NVARCHAR(128)
	, [Fax]					NVARCHAR(128)
	, [Website]				NVARCHAR(128)
)

--------------------------- INSERT INTO SOURCE COMPANY DETAILS ----------------------------------
INSERT INTO @SourceCompanyDetails (
	SourceIdentifier, 
	SourceIdentifierID1, 
	SourceIdentifierID2, 
	SourceIdentifierKey,
	OrganizationKey,
	StagingOrganizationKey,
    MatchScore,
	CreditRating)
	SELECT DISTINCT 
		CASE 
			WHEN SourceIdentifier = 'Staging Organization' THEN 'StagingOrganization'
			ELSE SourceIdentifier
		END AS SourceIdentifier,  
		SourceIdentifierID1, 
		SourceIdentifierID2, 
		SourceIdentifierKey ,
		OrganizationKey, 
		StagingOrganizationKey,
		(ROUND(RAND(CHECKSUM(NEWID())) * (2.75), 2)),
		(ROUND(RAND(CHECKSUM(NEWID())) * (3.0), 2))
		FROM OrganizationEventHistory 
		WHERE CorrelationId = @CorrelationID
IF NOT EXISTS (SELECT 1 FROM @SourceCompanyDetails) THROW 5100, 'No Source Company Details Found', 1

--------------------- DEFINE SOME COMPLIANCE DETAIL TYPES -----------------------
INSERT INTO @ComplianceCompanyDetails (ComplianceType, ComplianceUniqueId, DateID, [BusinessName], [Description], [Telephone], [Fax], [Website])
SELECT 'Sanction'             , 'C731699', '20180704','Sactions Test Company','Sections Test Description','222-222-222','222-222-222','www.sections.com'  UNION ALL
SELECT 'Adverse Media'        , 'C731686', '20180704','Adverse Media Test Company','Adverse Media Test Description','222-222-222','222-222-222','www.asverse.com' UNION ALL
SELECT 'Corporate/Business'   , 'C733339', '20180704','Corporate/Business Test Company','Corporate/Business Test Description','222-222-222','222-222-222','www.corp-bus.com' UNION ALL
SELECT 'Disqualified Director', 'C505030', '20180704','Disqualified Director Test Company','Disqualified Director Test Description','222-222-222','222-222-222','www.disqualified.com' UNION ALL
SELECT 'Financial Regulator'  , 'C731708', '20180704','Financial Regulator Test Company','Financial Regulator Test Description','222-222-222','222-222-222','www.financialregulator.com' UNION ALL
SELECT 'ID/V'                 , 'C732981', '20180704','ID/V Test Company','ID/V Test Description','222-222-222','222-222-222','www.idv.com' UNION ALL 
SELECT 'Insolvent'            , 'C505673', '20180704','Insolvent Test Company','Insolvent Test Description','222-222-222','222-222-222','www.Insolvent.com'UNION ALL
SELECT 'Law Enforcement'      , 'C733341', '20180704','Law Enforcement Test Company','Law Enforcement Test Description','222-222-222','222-222-222','www.lawenvorment.com' UNION ALL
SELECT 'PEP'                  , 'C732494', '20180704','PEP Test Company','PEP Test Description','222-222-222','222-222-222','www.PEP.com'

--------------------------- COMPLIANCE ARTICALS --------------------------------
MERGE Compliance.ComplianceCompanyArticle AS Target
USING (SELECT ComplianceType, ComplianceUniqueId FROM @ComplianceCompanyDetails) AS Source
ON (Source.ComplianceType = Target.[Source] AND Source.ComplianceUniqueId = Target.CompanyUniqueID )
WHEN NOT MATCHED BY TARGET THEN 
INSERT (
	[Source],
	CompanyUniqueID,
	DateID,
	CreatedByKey,
	CreatedDate,
	ModifiedByKey,
	ModifiedDate
)
VALUES
(
	Source.ComplianceType, 
	Source.ComplianceUniqueId,
	'20180704',
	@UserKey,
	GETDATE(),
	@UserKey,
	GetDate()
);

--------------------------- COMPLIANCE COMPANIES --------------------------------
MERGE Compliance.ComplianceCompany AS Target
USING (SELECT * FROM @ComplianceCompanyDetails) AS Source
ON (Source.ComplianceUniqueId = Target.[UniqueID] )
WHEN NOT MATCHED BY TARGET THEN 
INSERT (
	[DateID],
	[UniqueID],
	[BusinessName],
	[Description],
	[Telephone],
	[Fax],
	[Website],
	[Source],
	[SoftDelete],

	CreatedByKey,
	CreatedDate,
	ModifiedByKey,
	ModifiedDate
)
VALUES
(
	Source.[DateID],
	Source.[ComplianceUniqueId],
	Source.[BusinessName],
	Source.[Description],
	Source.[Telephone],
	Source.[Fax],
	Source.[Website],
	Source.[ComplianceType],
	'N',

	@UserKey,
	GETDATE(),
	@UserKey,
	GetDate()
);

--------------------------- COMPLIANCE ADDRESSES --------------------------------
MERGE [Compliance].[ComplianceCompanyAddress] AS Target
USING (SELECT * FROM @ComplianceCompanyDetails) AS Source
ON (Source.ComplianceUniqueId = Target.[CompanyUniqueID] )
WHEN NOT MATCHED BY TARGET THEN 
INSERT (
	[DateID],
	[CompanyUniqueID],
	[SoftDelete]
)
VALUES
(
	Source.[DateID],
	Source.[ComplianceUniqueId],
	'N'
);


------------------------------------- Org Enhanced Data -----------------------------------------------

------------------------------------- Staging Org Enhanced Data -----------------------------------------------
MERGE [dbo].[StagingOrganizationEnhancedData] AS Target
USING (SELECT * FROM @SourceCompanyDetails) AS Source
ON (Source.StagingOrganizationKey = Target.StagingOrganizationKey )
WHEN NOT MATCHED BY TARGET THEN 
INSERT (
	[StagingOrganizationKey],
	[CreditRating]
)
VALUES
(
	Source.[StagingOrganizationKey],
	Source.[CreditRating]
);

--------------------------- INSERT INTO COMPANY COMPLIANCE MAPPING --------------------------------
INSERT INTO Compliance.ComplianceCompanyMapping (
	SourceIdentifier, 
	SourceIdentifierId1, 
	SourceIdentifierId2, 
	SourceIdentifierKey,
	ComplianceUniqueID, 
	CreatedByKey, 
	ModifiedByKey, 
	MatchScore)
SELECT SCC.SourceIdentifier, 
	   SCC.SourceIdentifierID1, 
	   SCC.SourceIdentifierID2, 
	   SCC.SourceIdentifierKey, 
	   SCC.ComplianceUniqueId, 
	   @UserKey, 
	   @UserKey, 
	   SCC.MatchScore
FROM (
	SELECT SourceIdentifier, 
		   SourceIdentifierID1, 
		   SourceIdentifierID2, 
		   SourceIdentifierKey, 
		   ComplianceUniqueId, 
		   MatchScore
	FROM @SourceCompanyDetails AS CSD

	INNER JOIN @ComplianceCompanyDetails AS CCD
	ON CSD.ID = CCD.ID
) AS SCC
LEFT OUTER JOIN Compliance.ComplianceCompanyMapping AS CCM
ON      CCM.SourceIdentifier		       = SCC.SourceIdentifier
    AND CCM.SourceIdentifierId1            = SCC.SourceIdentifierID1
	AND isnull(CCM.SourceIdentifierId2,'') = isnull(SCC.SourceIdentifierID2,'')
	AND CCM.ComplianceUniqueID	           = SCC.ComplianceUniqueId
WHERE 
	CCM.SourceIdentifier			IS NULL
	AND CCM.SourceIdentifierId1 	IS NULL
	AND CCM.SourceIdentifierId2 	IS NULL
	AND CCM.ComplianceUniqueID		IS NULL


--------------------------- NORMALIZE MATCH SCORE IF NEEDED --------------------------------
-- Update Matches to come close to what we want
DECLARE @highervalueFromParameter NVARCHAR(50)
DECLARE @lowervalueFromParameter NVARCHAR(50)

DECLARE @highervalue FLOAT
DECLARE @lowervalue FLOAT
EXEC @highervalueFromParameter  =dbo.[fn_GetParameterValueByName] -1,'Higher Percentage','CounterpartyFileMatchingPercentage';
EXEC @lowervalueFromParameter = dbo.[fn_GetParameterValueByName] -1,'Lower Percentage','CounterpartyFileMatchingPercentage';

SELECT @highervalue = CAST(@highervalueFromParameter AS FLOAT)
	  ,@lowervalue = CAST(@lowervalueFromParameter AS FLOAT)

DECLARE  @Low_LowRange       INT = 0.0
DECLARE  @Low_HighRange      INT = @lowervalue - .001
DECLARE  @Medium_LowPercent  INT = @lowervalue
DECLARE  @Medium_HighPercent INT = @highervalue 
DECLARE  @High_LowPercent    INT = @highervalue
DECLARE  @High_HighPercent   INT = 2.75

DECLARE  @NumberOfRecordsPerChunk INT

SELECT @NumberOfRecordsPerChunk = COUNT(*)/3 FROM @ComplianceCompanyDetails 

Update CCM SET MatchScore = CASE 
								 WHEN SCC.ID >= ((@NumberOfRecordsPerChunk * 0) + 1) AND 
								      SCC.ID <  ((@NumberOfRecordsPerChunk * 1) + 1)
								      THEN  
									  ((SCC.MatchScore / 2.75) * (@Low_HighRange - @Low_LowRange)) + @Low_LowRange 
								 WHEN SCC.ID >= ((@NumberOfRecordsPerChunk * 1) + 1) AND 
								      SCC.ID <  ((@NumberOfRecordsPerChunk * 2) + 1)
								      THEN  
									  ((SCC.MatchScore / 2.75) * (@Medium_HighPercent - @Medium_LowPercent)) + @Medium_LowPercent 
								 WHEN SCC.ID >= ((@NumberOfRecordsPerChunk * 2) + 1) AND 
								      SCC.ID <  ((@NumberOfRecordsPerChunk * 3) + 1)
								      THEN  
									  ((SCC.MatchScore / 2.75) * (@High_HighPercent - @High_LowPercent)) + @High_LowPercent 
						      END
FROM Compliance.ComplianceCompanyMapping CCM INNER JOIN 
 (
	SELECT CSD.ID,
		   SourceIdentifier, 
		   SourceIdentifierID1,
		   SourceIdentifierID2, 
		   SourceIdentifierKey, 
		   ComplianceUniqueId, 
		   MatchScore
	FROM @SourceCompanyDetails AS CSD
	INNER JOIN @ComplianceCompanyDetails AS CCD
	ON CSD.ID = CCD.ID
) AS SCC
ON CCM.SourceIdentifier		= SCC.SourceIdentifier
	AND CCM.SourceIdentifierId1 = SCC.SourceIdentifierID1
	AND CCM.SourceIdentifierId2 = SCC.SourceIdentifierID2
	AND CCM.ComplianceUniqueID	= SCC.ComplianceUniqueId

--------------------------- PERFORM LINKING OF COMPLIANCE --------------------------------
MERGE OrganizationComplianceCompanyLinking as Target
USING 
(
	SELECT SourceIdentifier, 
		   SourceIdentifierID1,
		   SourceIdentifierID2,
		   ComplianceUniqueID,
		   @OrganizationKey AS OrganizationKey,
		   1 IsActive ,
		   @UserKey CreatedByKey,
		   GETDATE() CreatedDate,
		   @UserKey ModifiedByKey,
		   GETDATE() ModifiedDate
 FROM ( 
		SELECT CCM.SourceIdentifier, 
			   CCM.SourceIdentifierID1, 
			   CCM.SourceIdentifierID2 ,
			   ComplianceUniqueID,
			   Row_Number() OVER (PARTITION BY CCM.SourceIdentifier + ISNULL(CCM.SourceIdentifierID1,'') + ISNULL(CCM.SourceIdentifierID2,'') 
		ORDER BY MatchScore DESC )RowInfo
		FROM Compliance.ComplianceCompanyMapping CCM 
			 INNER JOIN OrganizationEventHistory OEH ON 
								CCM.SourceIdentifierId1        =             OEH.SourceIdentifierID1 AND 
					LEFT(ISNULL(CCM.SourceIdentifierId2,''),2) = LEFT(ISNULL(OEH.SourceIdentifierId2,''),2)
			 INNER JOIN AuditHistory AH              ON 
					AH.AuditText1 = OEH.CorrelationID 
		WHERE OEH.CorrelationID  = @CorrelationID
	) DT
	WHERE DT.RowInfo = 1 -- Basicly this is saying link the first compliance company map
) AS Source
ON 
(             Target.Source		   =	    Source.SourceIdentifier
   AND        Target.SourceID1     =		Source.SourceIdentifierID1
   AND ISNULL(Target.SourceID2,'') = ISNULL(Source.SourceIdentifierID2,'')
)
WHEN NOT MATCHED BY TARGET THEN 
INSERT (
	Source,
	SourceID1,
	SourceID2,
	ComplianceUniqueID,
	OrganizationKey,
	IsActive,
	CreatedByKey,
	CreatedDate,
	ModifiedByKey,
	ModifiedDate
)
VALUES
(
	Source.SourceIdentifier, 
	Source.SourceIdentifierID1,
	Source.SourceIdentifierID2,
	Source.ComplianceUniqueID,
	Source.OrganizationKey ,
	Source.IsActive ,
	Source.CreatedByKey,
	Source.CreatedDate,
	Source.ModifiedByKey,
	Source.ModifiedDate
);
---------------------------------------------------------------------------

---------------------------------------------------------------------------
DECLARE @PurchaseTypeKeyNew int
SELECT @PurchaseTypeKeyNew = PurchaseTypeKey
  FROM PurchaseType 
  where PurchaseTypedescription = 'Premium Monitoring'
---------------------------------------------------------------------------


------------------------ Compliance Purchases -----------------------------
INSERT INTO dbo.ComplianceCompanyPurchases (
	  PurchaserOrganizationKey, 
	  SourceIdentifier, 
	  SourceIdentifierId1, 
	  SourceIdentifierId2, 
	  SourceIdentifierKey, 
	  PurchaseTypeKey, 
	  StartDate, 
	  EndDate, 
	  CreatedDate, 
	  CreatedBy, 
	  CreatedByKey,
	  ModifiedDate, 
	  ModifiedBy, 
	  ModifiedByKey
)
SELECT @OrganizationKey, 
	   SCD.SourceIdentifier, 
	   SCD.SourceIdentifierId1, 
	   SCD.SourceIdentifierId2, 
	   SCD.SourceIdentifierKey, 
	   @PurchaseTypeKeyNew AS PurchaseTypeKey, 
	   GETDATE() - 40 AS StartDate, 
	   GETDATE() - 20 AS EndDate, 
	   GETDATE() AS CreatedDate, 
	   SUSER_NAME() AS CreatedBy, 
	   @UserKey AS CreatedByKey, 
	   GETDATE() AS ModifiedDate, 
	   SUSER_NAME() AS ModifiedBy, 
	   @UserKey AS ModifiedByKey
FROM (
	SELECT @OrganizationKey AS OrganizationKey, 
		   SourceIdentifier, 
		   SourceIdentifierId1, 
		   SourceIdentifierId2, 
		   SourceIdentifierKey, 
		   @PurchaseTypeKeyNew AS PurchaseTypeKey
	FROM @SourceCompanyDetails 
) AS SCD
LEFT OUTER JOIN ComplianceCompanyPurchases AS CCP
ON	CCP.PurchaserOrganizationKey = @OrganizationKey
AND CCP.SourceIdentifier		= SCD.SourceIdentifier
AND CCP.SourceIdentifierId1		= SCD.SourceIdentifierID1
AND CCP.SourceIdentifierId2		= SCD.SourceIdentifierID2
AND CCP.PurchaseTypeKey			= @PurchaseTypeKeyNew
WHERE 
CCP.PurchaserOrganizationKey IS NULL
AND CCP.SourceIdentifier		 IS NULL
AND CCP.SourceIdentifierId1		 IS NULL
AND CCP.SourceIdentifierId2		 IS NULL
AND CCP.PurchaseTypeKey			 IS NULL

------------------------------- Purchases ----------------------------------
INSERT INTO dbo.Purchase (
	PurchaserOrganizationKey, 
	PurchaseTypeKey, 
	PurchaseStartDate, 
	PurchaseEndDate, 
	IsActive, 
	CreatedByKey, 
	ModifiedByKey)
VALUES (@OrganizationKey, 
		@PurchaseTypeKeyNew, 
		GETDATE() - 40, 
		GETDATE() + 40, 
		1, 
		@UserKey, 
		@UserKey)

DECLARE @PurchaseKey INT = (SELECT SCOPE_IDENTITY())

---------------------------- Purchase Company ------------------------------
INSERT INTO dbo.PurchaseCompany (PurchaseKey, Source, SourceID1, IsActive, CreatedByKey, ModifiedByKey)
SELECT @PurchaseKey, SCD.SourceIdentifier, SCD.SourceIdentifierId1, 1, @UserKey, @UserKey
FROM (
	SELECT SourceIdentifier, SourceIdentifierId1
	FROM @SourceCompanyDetails 
) AS SCD
LEFT OUTER JOIN PurchaseCompany AS PC
ON	PC.PurchaseKey = @PurchaseKey
AND PC.Source		= SCD.SourceIdentifier
AND PC.SourceID1	= SCD.SourceIdentifierID1
WHERE 
PC.PurchaseCompanyKey IS NULL
 
---------------------------------------------------------------------------

SELECT 
	  SCD.SourceIdentifier                       AS SourceIdentifier
	, SCD.SourceIdentifierID1                    AS SourceIdentifierID1	
	, SCD.SourceIdentifierID2                    AS SourceIdentifierID2
	, SCD.SourceIdentifierKey                    AS SourceIdentifierKey
	, CCD.ComplianceType	                     AS ComplianceType
	, CCD.ComplianceUniqueId                     AS ComplianceUniqueId
	, SCD.MatchScore                             AS MatchScore
	, SCD.SourceIdentifierID1					 AS StagingOrganizationKey
	, CASE 
		WHEN CCP.PurchaserOrganizationKey IS NOT NULL 
		THEN CAST(1 AS BIT)
	    ELSE CAST(0 AS BIT)
	   END AS IsPurchased
	FROM @SourceCompanyDetails SCD
	INNER JOIN @ComplianceCompanyDetails AS CCD ON SCD.ID = CCD.ID
	LEFT JOIN ComplianceCompanyPurchases AS CCP ON
		    CCP.PurchaserOrganizationKey = @OrganizationKey
		AND CCP.SourceIdentifier		 = SCD.SourceIdentifier
		AND CCP.SourceIdentifierId1		 = SCD.SourceIdentifierID1
		AND CCP.SourceIdentifierId2		 = SCD.SourceIdentifierID2
		AND CCP.PurchaseTypeKey			 = @PurchaseTypeKeyNew
	

	

DECLARE @FileName NVARCHAR(200) = '$(FileName)' -- Provide new file name if you want to create data for new file
DECLARE @UserEmail NVARCHAR(200) = '$(UserEmail)'
DECLARE @UserKey BIGINT
SET @UserKey = '$(UserKey)'

-- Declarations -- 
DECLARE @OrganizationEventHistoryType [dbo].[OrganizationEventHistoryType]
DECLARE @MatchingAlgorithmTypeKey     BIGINT
DECLARE @CorrelationID                NVARCHAR(100)
DECLARE @AuditDetailsTypeKey          INT
DECLARE @MatchingProcessCompleteKey   INT
DECLARE @MatchingStatusKey            INT

-- Parameter Values -- 
SELECT @AuditDetailsTypeKey        = ParameterKey FROM Parameter WHERE ParameterValue    = 'TradeDirectory'
SELECT @MatchingProcessCompleteKey = ParameterKey FROM Parameter WHERE ParameterCategory = 'CounterpartyFileMatchingStatus' AND ParameterName = 'Matching Completed'
SELECT @MatchingStatusKey          = ParameterKey FROM Parameter WHERE ParameterCategory = 'CounterpartyFileMatchingStatus' AND ParameterName = 'Matching Pending'
SELECT @MatchingAlgorithmTypeKey   = ParameterKey FROM Parameter WHERE ParameterCategory = 'MatchingAlgorithmType' AND ParameterValue = 'ML'

-- Get Correlation ID
SELECT @CorrelationID = AuditText1 FROM dbo.AuditHistory WHERE AuditDetails = @FileName AND AuditDetailsTypeKey = @AuditDetailsTypeKey
IF @CorrelationID IS NULL THROW 5100, 'File Not found', 1

---- Cleanup
DELETE FROM OrganizationEventHistory WHERE CorrelationId = @CorrelationID
DELETE FROM CounterpartyOrgNoMatchReason WHERE CorrelationId = @CorrelationID
DELETE FROM StagingRawData WHERE CorrelationId = @CorrelationID

-- Populate Source Data
IF OBJECT_ID('tempdb..#SourceData') IS NOT NULL
	DROP TABLE #SourceData

-- Generate Organizations
SELECT TOP 120 ROW_NUMBER() OVER(ORDER BY StagingOrganizationKey DESC) SNo, 
	'Staging Organization' AS SourceIdentifier
	, StagingOrganizationKey AS SourceIdentifierID1
	, NULL AS SourceIdentifierId2
	, StagingOrganizationKey AS SourceIdentifierKey
	, OrganizationName AS name
	, Address1 AS street
INTO #SourceData 
FROM [dbo].[StagingOrganization]

-- Insert into staging raw data
INSERT INTO dbo.StagingRawData (CorrelationID, 
     EIN, 
     TaxCode, 
     IDFromNetwork, 
     IDFromBuyer, 
     DunsNumber, 
     ComplianceVendorID, 
     CompanyNumber, 
     OrganizationName,
     CurrentAlternativeLegalName, 
     Branch, 
     RegisteredBusinessName, 
     Address1, 
     Address2, 
     City, 
     State, 
     Zip, 
     Country, 
     CompanyGeoLatLong, 
     PhoneNumber, 
     FaxNumber, 
     CompanyURL,
     DateOfCompanyRegistration, 
     DateOfStartingOperation, 
     TypeOfOwnership, 
     ContactPersonName, 
     ContactPersonEmail, 
     ContactPersonPhone, 
     ContactPersonName2,
     ContactPersonEmail2, 
     ContactPersonPhone2, 
     CompanyBusinessDescription, 
     IndustryDescription, 
     IndustryCode, 
     BusinessClassification, 
     NumberOfEmployees, 
     Revenue,
     Certifications, 
     CertificateOfInsurance, 
     W9, 
     F1099, 
     ISOCertificates, 
     SOC1Type2, 
     IncorporationDocument, 
     JurisdictionCode,
     NormalisedName, 
     CompanyType, 
     Nonprofit, 
     CurrentStatus, 
     DissolutionDate, 
     BusinessNumber, 
     CurrentAlternativeLegalNameLanguage, 
     HomeJurisdictiontext, 
     NativeCompanyNumber,
     PreviousName, 
     AlternativeNames, 
     RetrievedAt, 
     RegistryUrl, 
     RestrictedForMarketing, 
     RegisteredAddressInFull, 
     PrimaryUserID, 
     CreatedDate, 
     CreatedBy, 
     CreatedByKey, 
     ModifiedDate,
     ModifiedBy, 
     ModifiedByKey, 
     StreetAddress, 
     Address3, 
     SpendPurchaseAmount, 
     PriorityLevel, 
     ParentCompanyName, 
     ParentCompanyAddress, 
     ParentCompanyAddress1, 
     ParentCompanyAddress2,
     ParentCompanyAddress3, 
     ParentCompanyCity, 
     ParentCompanyState, 
     ParentCompanyCountry, 
     ParentCompanyZip, 
     TIN, 
     VAT, 
     RegistrationNumber, 
     StreetAddress2, 
     City2, 
     State2, 
     Country2, 
     Zip2)
SELECT @CorrelationID AS CorrelationID, 
     EIN, 
     TaxCode, 
     IDFromNetwork, 
     IDFromBuyer, 
     DunsNumber, 
     ComplianceVendorID, 
     CompanyNumber, 
     OrganizationName,
     CurrentAlternativeLegalName, 
     NULL AS Branch, 
     RegisteredBusinessName, 
     Address1, 
     Address2, 
     City, 
     State, 
     Zip, 
     Country, 
     CompanyGeoLatLong, 
     PhoneNumber, 
     FaxNumber, 
     CompanyURL,
     DateOfCompanyRegistration, 
     DateOfStartingOperation, 
     TypeOfOwnership, 
     ContactPersonName, 
     ContactPersonEmail, 
     ContactPersonPhone, 
     ContactPersonName2,
     ContactPersonEmail2, 
     ContactPersonPhone2, 
     CompanyBusinessDescription, 
     IndustryDescription, 
     IndustryCode, 
     BusinessClassification, 
     NumberOfEmployees, 
     Revenue,
     Certifications, 
     CertificateOfInsurance, 
     W9, 
     F1099, 
     ISOCertificates, 
     SOC1Type2, 
     IncorporationDocument, 
     JurisdictionCode,
     NormalisedName, 
     CompanyType, 
     Nonprofit, 
     CurrentStatus, 
     DissolutionDate, 
     BusinessNumber, 
     CurrentAlternativeLegalNameLanguage, 
     HomeJurisdictiontext, 
     NativeCompanyNumber,
     PreviousName, 
     AlternativeNames, 
     RetrievedAt, 
     RegistryUrl, 
     RestrictedForMarketing, 
     RegisteredAddressInFull, 
     PrimaryUserID, 
     CreatedDate, 
     CreatedBy, 
     CreatedByKey, 
     ModifiedDate,
     ModifiedBy, 
     ModifiedByKey, 
     Address1 AS StreetAddress, 
     NULL AS Address3, 
     NULL AS SpendPurchaseAmount, 
     NULL AS PriorityLevel, 
     NULL AS ParentCompanyName, 
     NULL AS ParentCompanyAddress,
     NULL AS ParentCompanyAddress1, 
     NULL AS ParentCompanyAddress2, 
     NULL AS ParentCompanyAddress3, 
     NULL AS ParentCompanyCity, 
     NULL AS ParentCompanyState, 
     NULL AS ParentCompanyCountry,
     NULL AS ParentCompanyZip, 
     NULL AS TIN, 
     NULL AS VAT, 
     NULL AS RegistrationNumber, 
     NULL AS StreetAddress2, 
     NULL AS City2, 
     NULL AS State2, 
     NULL AS Country2, 
     NULL AS Zip2
FROM [dbo].[StagingOrganization] AS SO
INNER JOIN #SourceData AS SD
ON SO.StagingOrganizationKey = SD.SourceIdentifierKey
ORDER BY SO.OrganizationName

-- Populate our events
INSERT INTO @OrganizationEventHistoryType (
       RawOrganizationKey, 
	   StagingOrganizationKey, 
	   OrganizationKey, 
	   CorrelationID, 
	   MatchPercent, 
	   StagingOrganizationStatusKey, 
	   MatchingAlgorithmTypeKey, 
	   SelectedAsMatch, 
	   ModifiedDate, 
	   ModifiedBy, 
	   ModifiedByKey, 
	   CreatedDate, 
	   CreatedBy, 
	   CreatedByKey,
	   SourceIdentifier, 
	   SourceIdentifierID1, 
	   SourceIdentifierID2, 
	   SourceIdentifierKey,
	   Reason)
SELECT SO.RawOrganizationKey
        , S.StagingOrganizationKey
		, S.OrganizationKey 
		, SO.CorrelationID
		, ROUND(RAND(CHECKSUM(NEWID())) * (2.75), 2)
       , NULL
	   , @MatchingAlgorithmTypeKey
	   , 0
       , GETDATE()
	   , SUSER_NAME()
	   , NULL
	   , GETDATE()
	   , SUSER_NAME()
	   , NULL
       , 'Staging Organization' AS SourceIdentifier
       , S.StagingOrganizationKey AS SourceIdentifierID1
	   , NULL AS SourceIdentifierId2
       , S.StagingOrganizationKey AS SourceIdentifierKey
	   , NULL AS Reason
FROM dbo.StagingRawData AS SO
INNER JOIN dbo.StagingOrganization AS S
ON S.OrganizationName = SO.RegisteredBusinessName
WHERE SO.CorrelationID = @CorrelationID -- Only organizations for this file
ORDER BY RawOrganizationKey


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

SELECT @NumberOfRecordsPerChunk = COUNT(*)/3 FROM #SourceData 

UPDATE LOW 
	SET MatchPercent = ((MatchPercent / 2.0) * (@Low_HighRange - @Low_LowRange)) + @Low_LowRange -- Normalize the percent to fit in 'No Match'
FROM @OrganizationEventHistoryType LOW
JOIN #SourceData SD ON SD.SourceIdentifierID1 = LOW.SourceIdentifierID1
WHERE SNo >= ((@NumberOfRecordsPerChunk * 0) + 1) AND 
      SNo <  ((@NumberOfRecordsPerChunk * 1) + 1)
	
UPDATE MEDIUM 
	SET MatchPercent = ((MatchPercent / 2.0) * (@Medium_HighPercent - @Medium_LowPercent)) + @Medium_LowPercent -- Normalize the percent to fit in 'Likely match'
FROM @OrganizationEventHistoryType MEDIUM
JOIN #SourceData SD ON SD.SourceIdentifierID1 = MEDIUM.SourceIdentifierID1
WHERE SNo >= ((@NumberOfRecordsPerChunk * 1) + 1) AND 
	  SNo <  ((@NumberOfRecordsPerChunk * 2) + 1)

UPDATE HIGH 
	SET MatchPercent = ((MatchPercent / 2.0) * (@High_HighPercent - @High_LowPercent)) + @High_LowPercent -- Normalize the percent to fit in 'High match'
FROM @OrganizationEventHistoryType HIGH
JOIN #SourceData SD ON SD.SourceIdentifierID1 = HIGH.SourceIdentifierID1
WHERE SNo >= ((@NumberOfRecordsPerChunk * 2) + 1) AND 
      SNo <  ((@NumberOfRecordsPerChunk * 3) + 1)

EXEC [dbo].[usp_InsertUpdateOrganizationEventHistory] @OrganizationEventHistoryType 

-- Populate No Match Data
DECLARE @CounterpartyOrgNoMatchReasonType [dbo].[CounterpartyOrgNoMatchReasonType]
INSERT INTO @CounterpartyOrgNoMatchReasonType (
	[CorrelationID]			
	, [RawOrganizationKey]	
	, [Reason]				
	, [CreatedDate]			
	, [CreatedBy]			
	, [CreatedByKey]		
	, [ModifiedDate]		
	, [ModifiedBy]			
	, [ModifiedByKey]
)
SELECT 
	  sr.CorrelationID
	, sr.RawOrganizationKey
	, 'Non-Covered Jurisdiction'
	, GETDATE() AS [CreatedDate]
	, SUSER_NAME() AS [CreatedBy]
	, @UserKey AS [CreatedByKey]
	, GETDATE() AS [ModifiedDate]
	, SUSER_NAME() AS [ModifiedBy]
	, @UserKey AS [ModifiedByKey]
FROM dbo.StagingRawData  AS sr
INNER JOIN #SourceData AS sd
	ON sr.RegisteredBusinessName = sd.name 
INNER JOIN @OrganizationEventHistoryType AS LOW
	ON sd.SourceIdentifierID1 = LOW.SourceIdentifierID1
WHERE sr.CorrelationId = @CorrelationID AND
	  sd.SNo >= ((@NumberOfRecordsPerChunk * 0) + 1) AND 
      sd.SNo <  ((@NumberOfRecordsPerChunk * 1) + 1)

EXEC [dbo].[usp_InsertUpdateCounterpartyOrgNoMatchReason] @CounterpartyOrgNoMatchReasonType = @CounterpartyOrgNoMatchReasonType

-- Update Status
UPDATE AH 
SET AH.AuditText2 = @MatchingProcessCompleteKey,
    AH.AuditText3 = 'Matching Process Completed'
FROM AuditHistory AH
WHERE AH.AuditText1 = @CorrelationID

-- Add to ueue Table
DECLARE @Payload  [nvarchar](max)
SELECT @Payload=(SELECT TOP 1 * FROM @OrganizationEventHistoryType
FOR JSON AUTO,WITHOUT_ARRAY_WRAPPER)
DECLARE @QueueType [dbo].[QueueType]

-- Create Queue Item
INSERT INTO @QueueType (Queuekey, QueuePayloadType, QueuePayloadTypeVersion, Payload, QueueStatus, Comment, UserEmailId, CreatedByKey, ModifiedByKey)
	SELECT TOP 1 NULL AS Queuekey, 
					     'CounterpartyFileNoMatch' AS QueuePayloadType, 
						 '1' AS QueuePayloadTypeVersion,
						 @Payload AS Payload, 
						 'New' AS QueueStatus, 
						 'Mocked Matching' AS Comment, 
						 @UserEmail AS UserEmailId, 
						 @UserKey AS CreatedByKey, 
						 @UserKey AS ModifiedByKey 

-- Create Queue Item
exec usp_InsertUpdateQueue @QueueType 

SELECT @CorrelationID AS        CorrelationID,
	   'StagingOrganization' AS Source,
	   S.StagingOrganizationKey AS SourceId1,
	   NULL AS SourceId2,
	   S.StagingOrganizationKey AS StagingOrganizationKey 
	   FROM dbo.StagingRawData AS SO
	   INNER JOIN dbo.StagingOrganization AS S
	   ON S.OrganizationName = SO.RegisteredBusinessName
	   WHERE SO.CorrelationID = @CorrelationID -- Only organizations for this file
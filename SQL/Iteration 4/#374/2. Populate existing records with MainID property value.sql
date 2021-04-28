USE APPartment;

UPDATE obj
SET MainID = mainObj.[ID]
FROM [dbo].[Object] AS obj
INNER JOIN [dbo].[Chore] AS mainObj
ON obj.[ObjectID] = mainObj.[ObjectID]

UPDATE obj
SET MainID = mainObj.[ID]
FROM [dbo].[Object] AS obj
INNER JOIN [dbo].[Comment] AS mainObj
ON obj.[ObjectID] = mainObj.[ObjectID]

UPDATE obj
SET MainID = mainObj.[ID]
FROM [dbo].[Object] AS obj
INNER JOIN [dbo].[Event] AS mainObj
ON obj.[ObjectID] = mainObj.[ObjectID]

UPDATE obj
SET MainID = mainObj.[ID]
FROM [dbo].[Object] AS obj
INNER JOIN [dbo].[EventParticipant] AS mainObj
ON obj.[ObjectID] = mainObj.[ObjectID]

UPDATE obj
SET MainID = mainObj.[ID]
FROM [dbo].[Object] AS obj
INNER JOIN [dbo].[Home] AS mainObj
ON obj.[ObjectID] = mainObj.[ObjectID]

UPDATE obj
SET MainID = mainObj.[ID]
FROM [dbo].[Object] AS obj
INNER JOIN [dbo].[HomeSetting] AS mainObj
ON obj.[ObjectID] = mainObj.[ObjectID]

UPDATE obj
SET MainID = mainObj.[ID]
FROM [dbo].[Object] AS obj
INNER JOIN [dbo].[HomeStatus] AS mainObj
ON obj.[ObjectID] = mainObj.[ObjectID]

UPDATE obj
SET MainID = mainObj.[ID]
FROM [dbo].[Object] AS obj
INNER JOIN [dbo].[HomeUser] AS mainObj
ON obj.[ObjectID] = mainObj.[ObjectID]

UPDATE obj
SET MainID = mainObj.[ID]
FROM [dbo].[Object] AS obj
INNER JOIN [dbo].[Image] AS mainObj
ON obj.[ObjectID] = mainObj.[ObjectID]

UPDATE obj
SET MainID = mainObj.[ID]
FROM [dbo].[Object] AS obj
INNER JOIN [dbo].[Inventory] AS mainObj
ON obj.[ObjectID] = mainObj.[ObjectID]

UPDATE obj
SET MainID = mainObj.[ID]
FROM [dbo].[Object] AS obj
INNER JOIN [dbo].[Issue] AS mainObj
ON obj.[ObjectID] = mainObj.[ObjectID]

UPDATE obj
SET MainID = mainObj.[ID]
FROM [dbo].[Object] AS obj
INNER JOIN [dbo].[Message] AS mainObj
ON obj.[ObjectID] = mainObj.[ObjectID]

UPDATE obj
SET MainID = mainObj.[ID]
FROM [dbo].[Object] AS obj
INNER JOIN [dbo].[Notification] AS mainObj
ON obj.[ObjectID] = mainObj.[ObjectID]

UPDATE obj
SET MainID = mainObj.[ID]
FROM [dbo].[Object] AS obj
INNER JOIN [dbo].[NotificationParticipant] AS mainObj
ON obj.[ObjectID] = mainObj.[ObjectID]

UPDATE obj
SET MainID = mainObj.[ID]
FROM [dbo].[Object] AS obj
INNER JOIN [dbo].[ObjectLink] AS mainObj
ON obj.[ObjectID] = mainObj.[ObjectID]

UPDATE obj
SET MainID = mainObj.[ID]
FROM [dbo].[Object] AS obj
INNER JOIN [dbo].[ObjectParticipant] AS mainObj
ON obj.[ObjectID] = mainObj.[ObjectID]

UPDATE obj
SET MainID = mainObj.[ID]
FROM [dbo].[Object] AS obj
INNER JOIN [dbo].[Survey] AS mainObj
ON obj.[ObjectID] = mainObj.[ObjectID]

UPDATE obj
SET MainID = mainObj.[ID]
FROM [dbo].[Object] AS obj
INNER JOIN [dbo].[SurveyParticipant] AS mainObj
ON obj.[ObjectID] = mainObj.[ObjectID]

UPDATE obj
SET MainID = mainObj.[ID]
FROM [dbo].[Object] AS obj
INNER JOIN [dbo].[SurveyType] AS mainObj
ON obj.[ObjectID] = mainObj.[ObjectID]

UPDATE obj
SET MainID = mainObj.[ID]
FROM [dbo].[Object] AS obj
INNER JOIN [dbo].[User] AS mainObj
ON obj.[ObjectID] = mainObj.[ObjectID]
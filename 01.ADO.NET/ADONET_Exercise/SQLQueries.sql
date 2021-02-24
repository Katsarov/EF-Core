SELECT Id FROM Towns
WHERE Name = 'Sofia'

INSERT INTO Towns(Name, CountryCode)
VALUES(@townName, 1)

SELECT Id FROM Villains
WHERE Name = 'Gru'

INSERT INTO Villains(Name, EvilnessFactoriD)
VALUES(@villainName, @factorId)

INSERT INTO Minions(Name, Age, TownId)
VALUES(@minionName, @minionAge, @townId)

INSERT INTO MinionsVillains(MinionId, VillainId)
VALUES(@minionId, @villainId)

DELETE FROM MinionsVillains
WHERE MinionId = 1 AND VillainId = 1
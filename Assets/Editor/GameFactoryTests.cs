// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using NUnit.Framework;
using UnityEngine;
using CodeBase.Board.BoardKernel.Score;
using CodeBase.BoardItems.Cell;
using CodeBase.BoardItems.Token;
using CodeBase.EditorStructures;
using CodeBase.Services;
using CodeBase.Services.AssetService;

namespace Tests
{
    [TestFixture]
    public class GameFactoryTests
    {
        private GameFactory _gameFactory;
        private IAssetProvider _assetProviderMock;

        [SetUp]
        public void SetUp()
        {
            _assetProviderMock = new AssetProvider();
            _gameFactory = new GameFactory(_assetProviderMock);
        }

        [Test]
        public void CreateCell_ReturnsCellObject()
        {
            // Arrange
            var position = Vector3.zero;
            var parent = new GameObject().transform;
            var cellType = NodeType.Empty;
            var cellPrefab = new GameObject().AddComponent<Cell>();

            // Act
            var cell = _gameFactory.CreateCell(cellType, position, parent);

            // Assert
            Assert.IsNotNull(cell);
            Assert.AreEqual(position, cell.transform.position);
            Assert.AreEqual(parent, cell.transform.parent);
        }

        [Test]
        public void CreateToken_ReturnsTokenObject()
        {
            // Arrange
            var position = Vector3.zero;
            var parent = new GameObject().transform;
            var tokenPrefab = new GameObject().AddComponent<Token>();
            // Act
            var token = _gameFactory.CreateToken(position, parent);

            // Assert
            Assert.IsNotNull(token);
            Assert.AreEqual(position, token.Transform.position);
            Assert.AreEqual(parent, token.Transform.parent);
        }

        [Test]
        public void Reclaim_ReclaimsScoreItem()
        {
            // Arrange
            var scoreItem = new GameObject().AddComponent<ScoreItem>();
            scoreItem.Initialize(_gameFactory);

            // Act
            _gameFactory.Reclaim(scoreItem);

            // Assert
            Assert.AreEqual(Vector3.zero, scoreItem.transform.position);
            // Additional assertions if necessary
        }

        [Test]
        public void Reclaim_ReclaimsToken()
        {
            // Arrange
            var token = new GameObject().AddComponent<Token>();
            token.gameObject.SetActive(true);
            token.Transform.position = Vector3.one;

            // Act
            _gameFactory.Reclaim(token);

            // Assert
            Assert.AreEqual(Vector3.zero, token.Transform.position);
            Assert.IsFalse(token.gameObject.activeSelf);
            // Additional assertions if necessary
        }

        // Add more test methods as needed to cover other scenarios and functionalities of the GameFactory class
    }
}
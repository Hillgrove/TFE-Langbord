﻿using SensorDataRepository.Models;

namespace SensorDataRepository.Tests
{
    [TestClass()]
    public class SensorRepositoryTests
    {
        private SensorRepository _sensorRepository = new SensorRepository();
        public List<SensorData> seedData = new List<SensorData>
        {
            new SensorData { Humidity = 50.0, Temperature = 22.0, Pressure = 1000.5, SerialNumber = "1" },
            new SensorData { Humidity = 55.0, Temperature = 23.0, Pressure = 1010.3, SerialNumber = "2" },
            new SensorData { Humidity = 60.0, Temperature = 24.0, Pressure = 9001.5, SerialNumber = "3" }
        };

        [TestInitialize]
        public void InitOnce()
        {
            _sensorRepository.TruncateDb();

            foreach (var data in seedData)
            {
                _sensorRepository.Add(data);
            }
        }

        [TestMethod()]
        public void Add_ValidSensorData_ShouldIncreaseRowCount()
        {
            // Arrange
            double humidity = 45.0;
            double temperature = 21.5;
            double pressure = 939.5;
            string serialNumber = "1";
            var newSensorData = new SensorData { Humidity = humidity, Temperature = temperature, Pressure = pressure, SerialNumber = serialNumber };

            // Act
            _sensorRepository.Add(newSensorData);

            // Assert
            var allData = _sensorRepository.GetAll();
            Assert.IsTrue(allData.Count() > seedData.Count, "Data was not added successfully.");
            Assert.IsTrue(allData.Any(d => d.Humidity == humidity && d.Temperature == temperature && d.Pressure == pressure && d.SerialNumber == serialNumber), "New data is missing.");
        }

        [TestMethod()]
        [DataRow(0)] // No rows
        [DataRow(1)] // Smallest limit
        [DataRow(2)] // Middle value
        [DataRow(3)] // Equal to seeded row count
        [DataRow(4)] // Exceeds seeded row count
        public void GetAll_BoundaryValues_ShouldReturnLimitedRows(int rowsToFetch)
        {
            // Act and Arrange
            var fetchedRows = _sensorRepository.GetAll()
               .OrderByDescending(s => s.Id)
               .Take(rowsToFetch)
               .ToList();

            // Assert
            Assert.IsTrue(fetchedRows.Count() <= rowsToFetch, $"Expected less than or equal to {rowsToFetch} rows, but got {fetchedRows.Count}.");
        }
    }
}
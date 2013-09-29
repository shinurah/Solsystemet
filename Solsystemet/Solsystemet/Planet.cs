﻿/**************************************************************
 * Datamaskingrafikk - Obligatorisk innlevering 3 - Solsystem
 * Lisa Marie Sørensen(500973) og Mikael Bendiksen(500694)
 * Planet.cs
 **************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Solsystemet
{
    class Planet
    {
        private Moon[] moonArray;

        private String planetName;
        private Model planetModel;
        private Texture2D planetTexture;

        public Texture2D PlanetTexture
        {
            get { return planetTexture; }
            set { planetTexture = value; }
        }

        private Vector3 planetDistanceToSun;

        private Vector3 planetPosition;

        private float[] planetScale;


        private float planetRotSpeed;
        private float planetRotY = 0.0f;

        private float planetOrbitSpeed;
        private float planetOrbitY = 0.0f;


        public Planet(String name, Model model, float[] scale, Vector3 distance, float rotSpeed, float orbitSpeed, int moonArraySize)
        {
            this.planetName = name;
            this.planetModel = model;
            this.planetScale = scale;
            this.planetDistanceToSun = distance;
            this.planetPosition = distance;
            this.planetRotSpeed = rotSpeed;
            this.planetOrbitSpeed = orbitSpeed;
            this.moonArray = new Moon[moonArraySize];

        }

        public String PlanetName
        {
            get { return planetName; }
            set { planetName = value; }
        }

        public Model PlanetModel
        {
            get { return planetModel; }
            set { planetModel = value; }
        }

        public Vector3 PlanetDistanceToSun
        {
            get { return planetDistanceToSun; }
            set { planetDistanceToSun = value; }
        }

        public float PlanetRotY
        {
            get { return planetRotY; }
            set { planetRotY = value; }
        }

        public float PlanetOrbitSpeed
        {
            get { return planetOrbitSpeed; }
            set { planetOrbitSpeed = value; }
        }

        public float PlanetRotSpeed
        {
            get { return planetRotSpeed; }
            set { planetRotSpeed = value; }
        }

        public float PlanetOrbitY
        {
            get { return planetOrbitY; }
            set { planetOrbitY = value; }
        }

        internal Moon[] MoonArray
        {
            get { return moonArray; }
            set { moonArray = value; }
        }

        public float[] PlanetScale
        {
            get { return planetScale; }
            set { planetScale = value; }
        }

        public Vector3 PlanetPosition
        {
            get { return planetPosition; }
            set { planetPosition = value; }
        }

    }
}
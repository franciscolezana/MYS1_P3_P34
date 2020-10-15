using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//IMPORTS
using SimioAPI;
using Simio.SimioEnums;
using SimioAPI.Extensions;
using SimioAPI.Graphics;
using Simio;

namespace P3_P34
{
    class ApiSimio
    {
        private ISimioProject proyectoApi;
        private string rutabase = "Modelobase.spfx";
        private string rutafinal = "ModeloFinal.spfx";
        private string[] warnings;
        private IModel model;
        private IIntelligentObjects intelligentObjects;

        public ApiSimio()
        {
            proyectoApi = SimioProjectFactory.LoadProject(rutabase, out warnings);
            model = proyectoApi.Models[1];
            intelligentObjects = model.Facility.IntelligentObjects;
        }


        public void crearModelo()
        {
            //Source
            intelligentObjects.CreateObject("Source", new FacilityLocation(0, 0, 5)); //x,z,y
            intelligentObjects.CreateObject("Source", new FacilityLocation(0, 0, 10)); //x,z,y
            intelligentObjects.CreateObject("Source", new FacilityLocation(0, 0, 15)); //x,z,y
            model.Facility.IntelligentObjects["Source1"].Properties["InterarrivalTime"].Value = "Random.Exponential(5)";
            SimioProjectFactory.SaveProject(proyectoApi, rutafinal, out warnings);
        }
    }

}

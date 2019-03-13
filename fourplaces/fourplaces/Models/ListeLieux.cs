using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using Plugin.Geolocator.Abstractions;

namespace fourplaces.Models
{
    public class ListeLieux
    {
        public List<PlaceItemSummary> Lieux { get; set; }

        public ListeLieux()
        {
            Lieux = new List<PlaceItemSummary>()
            {
            };
        }

        public ListeLieux(List<PlaceItemSummary> list)
        {
            Lieux = list;
        }

        public void SortDistance()
        {
            foreach (PlaceItemSummary element in Lieux)
            {
                element.calculPos();
            }
            Lieux.Sort();
        }


    }
}

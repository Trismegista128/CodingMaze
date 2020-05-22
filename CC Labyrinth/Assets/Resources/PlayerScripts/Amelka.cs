using UnityEngine;
using Assets.Scripts;
using System.Linq;
using System.Collections.Generic;

namespace Assets.Resources.PlayerScripts
{
    public class Amelka : MonoBehaviour, IPlayerAI
    {
        private DirectionType ostatniRuch;
        private bool CzyToJestMojPierwszyRuch = true;

        public DirectionType RequestMove(DirectionType[] opcje)
        {
            if (CzyToJestMojPierwszyRuch == true)
            {
                CzyToJestMojPierwszyRuch = false;
                ostatniRuch = opcje.First();
                return ostatniRuch;
            }

            var powrot = KtoryRuchToPowrot(ostatniRuch);
            var wybor = WybierzPierwszaInnaOpcjeNizPrzeciwna(opcje, powrot);
            ostatniRuch = wybor;

            return ostatniRuch;
        }

        public DirectionType KtoryRuchToPowrot(DirectionType kierunek)
        {
            if (kierunek == DirectionType.Left) return DirectionType.Right;
            if (kierunek == DirectionType.Right) return DirectionType.Left;
            if (kierunek == DirectionType.Up) return DirectionType.Down;

            return DirectionType.Up;
        }

        public DirectionType WybierzPierwszaInnaOpcjeNizPrzeciwna(DirectionType[] opcje, DirectionType powrot)
        {
            var listaDobrychOpcji = opcje.Where(x=> x != powrot);
            return listaDobrychOpcji.First();
        }
    }
}

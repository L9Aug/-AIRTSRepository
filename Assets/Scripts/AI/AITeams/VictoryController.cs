using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryController : MonoBehaviour
{
    enum VictoryTypes { Economic, Cultural, Military };

    public enum MilitaryVictoryTypes { DestroyHQ, DestroyAllBuildings, DestroyEverything };

    public static VictoryController VC;

    public TeamManager TM;

    public int EconomicTarget;

    public int CulturalTarget;

    public MilitaryVictoryTypes CurrentMilitaryCondition;

    public List<int> EconomicScores;

    public List<int> CulturalScores;

    List<int> RemainingTeams;

    private void Start()
    {
        VC = this;
    }

    public void InitialiseVictoryConditions(int numTeams)
    {
        EconomicScores = new List<int>();
        CulturalScores = new List<int>();
        RemainingTeams = new List<int>();

        for (int i = 0; i < numTeams; ++i)
        {
            EconomicScores.Add(0);
            CulturalScores.Add(0);
            RemainingTeams.Add(i);
        }
    }

    public void AddEconomicScore(int Amount, int Team)
    {
        EconomicScores[Team] += Amount;

        // do victory checks here.
        if (EconomicScores[Team] >= EconomicTarget)
        {
            InitiateVictory(Team, VictoryTypes.Economic);
        }
    }

    public void AddCulturalScore(int Amount, int Team)
    {
        CulturalScores[Team] += Amount;

        // do victory checks
        if (CulturalScores[Team] >= CulturalTarget)
        {
            InitiateVictory(Team, VictoryTypes.Cultural);
        }
    }

    public void UpdateMilitary(int Team)
    {
        if (HasTeamLost(Team))
        {
            RemainingTeams.Remove(Team);
            DisableTeam(Team);
        }

        if(RemainingTeams.Count == 1)
        {
            InitiateVictory(RemainingTeams[0], VictoryTypes.Military);
        }
    }

    bool HasTeamLost(int Team)
    {
        switch (CurrentMilitaryCondition)
        {
            case MilitaryVictoryTypes.DestroyHQ:
                if (TeamManager.TM.Teams[Team].BuildingsList.Find(x => x.BuildingType == Buildings.TownHall) == null)
                {
                    return true;
                }
                break;

            case MilitaryVictoryTypes.DestroyAllBuildings:
                if (TeamManager.TM.Teams[Team].BuildingsList.Count == 0)
                {
                    return true;
                }
                break;

            case MilitaryVictoryTypes.DestroyEverything:
                if (TeamManager.TM.Teams[Team].BuildingsList.Count == 0 &&
                    TeamManager.TM.Teams[Team].Population.Citizens.Count == 0 &&
                    TeamManager.TM.Teams[Team].Population.Merchants.Count == 0 &&
                    TeamManager.TM.Teams[Team].Population.Military.Count == 0)
                {
                    return true;
                }
                break;
        }

        return false;
    }

    void DisableTeam(int Team)
    {
        TeamManager.TM.Teams[Team].isTeamActive = false;
        // Destroy all units and buildings on the map.
    }

    void InitiateVictory(int Team, VictoryTypes VictoryMode)
    {

    }

}

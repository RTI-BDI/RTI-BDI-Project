using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class Domain
{
    public string name;
    public List<string> requirements;
    public List<Type> types;
    public List<Belief> beliefs;
    public List<Action> actions;

    public Domain(string name, List<Type> types, List<Belief> beliefs, List<Action> actions)
    {
        this.name = name;
        this.requirements = InitializeRequirements(); 
        this.types = types;
        this.beliefs = beliefs;
        this.actions = actions;
    }

    private List<string> InitializeRequirements()
    {
        List<string> requirements = new List<string>();
        requirements.Add(":strips");
        requirements.Add(":durative-actions");
        requirements.Add(":fluents");
        requirements.Add(":duration-inequalities");
        requirements.Add(":typing");
        requirements.Add(":continuous-effects");
        return requirements;
    }

    public static Domain Evaluate(JToken token)
    {
        if(token.First.ToString() == "define_domain")
        {
            //2nd element
            string name = token.First.Next.ToString();

            //3rd element
            List<Type> types = new List<Type>();
            foreach(JToken j in token.First.Next.Next)
            {
                if(j != token.First.Next.Next.First)
                {
                    types.Add(Type.Evaluate(j));
                }
            }

            //4th element and 5th
            List<Belief> beliefs = new List<Belief>();
            foreach (JToken j in token.First.Next.Next.Next)
            {
                if (j != token.First.Next.Next.Next.First)
                {
                    beliefs.Add(Belief.Evaluate(j));
                }
            }
            foreach (JToken j in token.First.Next.Next.Next.Next)
            {
                if (j != token.First.Next.Next.Next.Next.First)
                {
                    beliefs.Add(Belief.Evaluate(j));
                }
            }

            //6th element
            List<Action> actions = new List<Action>();
            foreach (JToken j in token.First.Next.Next.Next.Next.Next)
            {
                if (j != token.First.Next.Next.Next.Next.Next.First)
                {
                    actions.Add(Action.Evaluate(j));
                }
            }

            return new Domain(name, types, beliefs, actions);


        } else {
            return null;
        }
    }

    public string ToPDDL()
    {
        string pddl = "";

        //Domain name
        pddl = pddl + "(define (domain " + this.name + ")\n";

        //Requirements
        pddl = pddl + "(:requirements ";
        foreach (string s in this.requirements)
        {
            pddl = pddl + s + " ";
        }
        pddl = pddl + ")\n";

        //Types
        pddl = pddl + "(:type\n";
        foreach(Type t in this.types)
        {
            pddl = pddl + t.ToPDDL();
        }
        pddl = pddl + ")\n";

        //Predicates
        pddl = pddl + "(:predicates \n";
        foreach(Belief b in this.beliefs.FindAll(e => e.type == Belief.BeliefType.Predicate))
        {
            pddl = pddl + b.ToPDDL(true) + "\n";
        }
        pddl = pddl + ")\n";

        //Functions
        pddl = pddl + "(:functions \n";
        foreach(Belief b in this.beliefs.FindAll(e => (e.type == Belief.BeliefType.Function || e.type == Belief.BeliefType.Constant)))
        {
           pddl = pddl + b.ToPDDL(true) + "\n";
        }
        pddl = pddl + ")\n";

        //Actions
        foreach(Action a in this.actions)
        {
            pddl = pddl + a.ToPDDL();
        }

        return pddl;
    }

}

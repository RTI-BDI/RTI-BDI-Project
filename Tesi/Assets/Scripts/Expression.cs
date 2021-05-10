using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class Expression
{
    //Representing the node of the expression 'tree'
    public Node node;
    //Left branch
    public Expression exp_1;
    //Right branch
    public Expression exp_2;

    //Initialization of operator 
    public Expression(Node node, Expression exp_1, Expression exp_2)
    {
        this.node = node;
        this.exp_1 = exp_1;
        this.exp_2 = exp_2;
    }

    //Initialization of unary operator
    public Expression(Node node, Expression exp_1)
    {
        this.node = node;
        this.exp_1 = exp_1;
        this.exp_2 = null;
    }

    //Initialization of a leaf (value or belief)
    public Expression(Node node)
    {
        this.node = node;
        this.exp_1 = null;
        this.exp_2 = null;
    }

    //Transalte from JSON to Object
    public static Expression Evaluate(JToken token)
    {
        Node node = Node.Evaluate(token);
        switch (node.type)
        {
            case (Node.NodeType.Operator):
                return new Expression(node, Expression.Evaluate(token.First.Next), Expression.Evaluate(token.First.Next.Next));
                break;
            case (Node.NodeType.Unary):
                return new Expression(node, Expression.Evaluate(token.First.Next));
                break;
            case (Node.NodeType.LeafValue):
                return new Expression(node);
                break;
            case (Node.NodeType.LeafBelief):
                return new Expression(node);
                break;
            default:
                return null;
                break;
        }
    }

    //Translate from Object to PDDL
    public string ToPDDL(bool questionMark)
    {
        if(this.node.type == Node.NodeType.Operator)
        {
            return "(" + this.node.ToPDDL(questionMark) + " " + this.exp_1.ToPDDL(questionMark) + " " + this.exp_2.ToPDDL(questionMark) + ")";
        } else if (this.node.type == Node.NodeType.Unary) {
            return "(" + this.node.ToPDDL(questionMark) + " " + this.exp_1.ToPDDL(questionMark) + ")";
        } else {
            return this.node.ToPDDL(questionMark);
        }
    }

}

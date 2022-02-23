using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Families
{
    public class FamiliesHelper
    {
        private int DistinctFamilies(List<Tuple<string, string>> relations)
        {
            if (relations == null || relations.Count == 0)
            {
                return 0;
            }

            int count = 0;
            while (relations.Count > 0)
            {
                var relation = relations.FirstOrDefault();
                if (relation != null)
                {
                    RemoveRelativesByRelation(relation, ref relations);
                    count++;
                }
            }

            return count;
        }

        private void RemoveRelativesByRelation(Tuple<string, string> relation, ref List<Tuple<string, string>> relations)
        {
            relations.Remove(relation);
            var memberRelations = relations
                .Where(x =>
                    x.Item1 == relation.Item1 ||
                    x.Item1 == relation.Item2 ||
                    x.Item2 == relation.Item1 ||
                    x.Item2 == relation.Item2)
                .Select(x => x)
                .Distinct()
                .ToList();

            relations = relations.Except(memberRelations).ToList();

            var relationsToRemove = new List<Tuple<string, string>>(memberRelations);
            for (var i = 0; i < memberRelations.Count; i++)
            {
                var memberRelation = memberRelations[i];
                var memberRelationRelations = relations
                    .Where(x =>
                        x.Item1 == memberRelation.Item1 ||
                        x.Item1 == memberRelation.Item2 ||
                        x.Item2 == memberRelation.Item1 ||
                        x.Item2 == memberRelation.Item2)
                    .Select(x => x)
                    .Distinct()
                    .ToList();
                relationsToRemove.AddRange(memberRelationRelations);
                relationsToRemove = relationsToRemove.Distinct().ToList();
                var relationsToKeepLooking = relationsToRemove.Except(memberRelations).ToList();
                memberRelations.AddRange(relationsToKeepLooking);
            }
            relations = relations.Except(relationsToRemove).ToList();
        }
    }
}

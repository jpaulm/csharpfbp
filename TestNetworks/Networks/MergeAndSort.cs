namespace {    // change this if you want 
 public class MergeandSort : Network {
string description = "Merge and Sort Network";
 public override void Define() { 
Component("Write_text__to_pane", typeof(com.jpaulmorrison.fbp.core.components.misc.WriteToConsole)); 
Component("Sort", typeof(com.jpaulmorrison.fbp.core.components.routing.Sort)); 
Component("Generate____1st_group", typeof(com.jpaulmorrison.fbp.core.components.misc.GenerateTestData)); 
Component("Generate___2nd_group", typeof(com.jpaulmorrison.fbp.core.components.misc.GenerateTestData)); 
Connect(Component("Sort"), Port("OUT"), Component("Write_text__to_pane"), Port("IN")); 
Connect(Component("Generate___2nd_group"), Port("OUT"), Component("Sort"), Port("IN")); 
Connect(Component("Generate____1st_group"), Port("OUT"), Component("Sort"), Port("IN")); 
Initialize("100", Component("Generate___2nd_group"), Port("COUNT")); 
Initialize("100", Component("Generate____1st_group"), Port("COUNT")); 

 } 
internal static void main(String[] argv) { 
 new MergeandSort().Go();
 }
} 
 
}

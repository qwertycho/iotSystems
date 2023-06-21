
using Microsoft.AspNetCore.Mvc;
using iotServer.classes;
using iotServer.Models;
// loggen
// using Microsoft.Extensions.Logging;

namespace iotServer.Controllers
{
    public class groupsController: Controller
    {
      private readonly ILogger<groupsController> _logger;
      private GroupModel model = new GroupModel();

      public groupsController(ILogger<groupsController> logger )
      {
        _logger = logger;
      }

      public async Task<IActionResult> Index()
      {
        ViewBag.groups = await model.GetAllGroups();
        return View();
      }

      public async Task<IActionResult> Update(string id, string color, string name)
      {
        try{
        int groupID = int.Parse(id);

          await model.Update(groupID, color, name);

          return Redirect("/groups");
        }catch(Exception e){
          _logger.LogError(e.Message);
          return Redirect("/groups");
        }
      }

      public async Task<IActionResult> newGroup(string name, string color)
      {
        try{
          await model.newGroup(name, color);
        }catch(Exception e){
          _logger.LogError(e.Message);
        }
        return RedirectToAction("Index");
      }

      public async Task<IActionResult> deleteGroup(string id)
      {
        try{
          int groupID = int.Parse(id);
          await model.deleteGroup(groupID);
        }catch(Exception e){
          _logger.LogError(e.Message);
        }
        return RedirectToAction("Index");
      }
    }


}

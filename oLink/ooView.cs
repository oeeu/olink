using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oLink
{
    class ooView
    {
        static public string sShow = @"
<div class='main'>
  <div class='maia'>
    <div class='lisc' style='padding:0 0 15px 0;'>
      <div class='artl'>
^文章^
      </div>
    </div>
  </div>
  <div style='height:36px; clear:both;'></div>
</div>";

        static public string sCard = @"<div class='lisc'>
            <div class='picr'><a href=""javascript:void(0);"" onclick=""javascript:Load(encodeURI('c^序号^'));""><img class='imgh' src='^封面^'></a></div>
            <div>
                <div class='namc'><a href=""javascript:void(0);"" onclick=""javascript:Load(encodeURI('c^序号^'));""><font class='dark'>^标题^</font></a></div>
                <div class='conc'>
                    <p>^摘要^</p>
                </div>
            </div>
        </div>";

        static public string sHome = @"
<div class='main'>
  <div class='maia'>
^卡片^
  </div>
  <div class='maib'>
        <div class='lisc' style='padding:0;'>
            <img style='width:100%' src='../File/ooHead.jpg'>
            <img style='width:120px; border-radius:50%; margin:-64px 0 0 15px; border:4px solid #ffffff; background:#ffffff;' src='../File/ooFace.jpg'>
            <div style='padding:0 15px 12px 15px;'><font class='large'>^Name^</font>
                <p>^Note^</p>
            </div>
        </div>
  </div>
  <div style='clear:both;'></div>
</div>";
    }
}

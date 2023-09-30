using System.IO;
using System.Linq;
using AstroCreate.Utilities;
using Godot;
using SimaiSharp;
using SimaiSharp.Structures;

namespace AstroCreate;

public partial class Gameplay : Control
{
    public static string CHART_TEXT = @"&title=VeRForTe αRtE:VEiN
&artist=orangentle
&first=1.065
&des_4=ロシェ@ペンギン
&des_5=翡翠マナ -Memoir-
&lv_2=6
&lv_3=8
&lv_4=12+
&lv_5=14+
&inote_2=
(190){1},
{1}1h[1:1],
{1},
{1}8h[1:1],
{1},
{1}1h[1:1]/8h[1:1],
{1},
{1}1x/8x,
{2}1x,1x,
{4}1x/2x,,,8x,
{4},8x,,7x/8x,
{1},
{4},1,,1,
{4},1/8,,,
{4},B1/B8,,,
{4},,,C1h[8:7]/B2/B7,
{1},
{4},,,1xh[1:1]/2xh[1:1],
{1},
{4},,,3h[1:1]/4h[1:1],
{1},
{4},,,5h[1:1]/6h[1:1],
{1},
{4},,,7h[1:1]/8h[1:1],
{1},
{4},,,8,
{4},8,,7,
{4},7,,6,
{4},6,,5/6,
{4},,,4,
{4},4,,3,
{4},3,,2x,
{4},2x,,1x/2x,
{4},,,B1,
{4},,,B8,
{4},,,B2,
{4},,,B7,
{4},,,B3,
{4},,,B6,
{4},,,B4,
{4},,,C1,
{4},,,1x,
{4},1x,,2,
{4},2,,3/4,
{4},3/4,,3/4,
{4},,,5,
{4},5,,6,
{4},6,,7x/8x,
{4},7x/8x,,7x/8x,
{4},,,1x/2x,
{4},1x/2x,,1x/2x,
{4},,,8-5[4:3],
{1},
{4},,,4-1[4:3],
{1},
{4},,,8-3[4:3],
{1},
{4},,,1-6[4:3],
{1},
{4},,,E8,
{4},,,E7,
{4},,,E6,
{4},,,E5,
{4},,,E4,
{4},,,E3,
{4},,,E2,
{4},,,E1,
{4},,,B1/B8,
{4},,,C1h[8:7]/B2/B7,
{1},
{4},,,1b/8b,
{4},,,1x,
{4}1,1,1,2,
{4}2,1/2,,3,
{4}3,3,3,4,
{4}4,4/5,,5,
{4}5,5,5,6,
{4}6,5/6,,7,
{4}7,7,7,8,
{4}8,1,1,8bx,
{4}8,7,7,6x,
{4}6x,5x/6x,,4,
{4}4,3,3,2,
{4}2,1/2,,1,
{4}1,2,2,3,
{4}3,4/5,4/5,5,
{4}5,6,6,7,
{4}7,1bx/8bx,,8-5[4:3],
{1},
{4},,,4-1[4:3],
{1},
{4},,,8,
{4}8,8,,7,
{4}7,7/8,,1,
{4}1,1,,2,
{4}2,1/2,,8x,
{4}8,8,8,1,
{4}1,1,1,8,
{4}7,6,5,4,
{4}3,2,1,1/8,
{4}1/8,2/7,2/7,3/6,
{4}3/6,4/5,4/5,4-1[4:3]/5-8[4:3],
{1},
{4},,,C1fh[2:3],
{1},
{4},B2/B7,,,
{1},
{1},
{1},
E
&inote_3=
(190){1},
{1},
{1},
{1}4h[16:15]/5h[16:15],
{1},
{4}3,3,3/4,,
{4}6,6,5/6,,
{4}4/5,4/5,4b/5b,,
{2}3h[2:1],4h[2:1],
{4}5,,6x,6xh[2:1],
{4},5h[2:1],,4h[2:1],
{4},3,,2x,
{4}2x,2xh[2:1],,1h[2:1],
{4},8,,7,
{4}7,5xh[8:5]/6xh[8:5],,,
{8},,,5bx/6bx,,,3xh[8:5]/4xh[8:5],,
{8},,,,,,,3bx/4bx,
{4},,,2-5[2:1],
{4},,,7-4[2:1],
{4},,,2-6[2:1],
{4},,,7h[2:1],
{4},8,,7-4[2:1],
{4},,,2-5[2:1],
{4},,,7-3[2:1],
{4},,,2h[2:1],
{4},1,,7/8,
{4},7/8,,5/6,
{4},5/6,,3/4,
{4},3/4,,1/2,
{4},1/2,,8,
{8}8,,8,,1x,1x,,,
{8},1x/2x,,,,3x/4x,,,
{8},5x/6x,,,,7x/8x,,,
{8},1bx/8bx,,,,,2,,
{4}2,3,3,4,
{4}4,5/6,,7,
{4}7,6,6,5,
{4}5,3/4,,3,
{4}3,2,2,1,
{4}1,7/8,,6,
{4}6,7,7,8,
{4}8,1/8,,B1,
{4}B2,B3,B4,B5,
{4}B6,C1,,B8,
{4}B7,B6,B5,B4,
{4}B3,C1,,1-6[2:1]/2-5[2:1],
{4},,,7-4[2:1]/8-3[2:1],
{4},,,1xh[8:3]/2xh[8:3],
{8},,,,,3xh[2:1]/4xh[2:1],,,
{8},,,,,5xh[2:1]/6xh[2:1],,,
{8},,,,,7xh[2:1]/8xh[2:1],,,
{8},,,,,,1/2,1/2,
{4},1<6[4:3]/2>5[4:3],,,
{8},,,,,,7/8,7/8,
{4},7<4[4:3]/8>3[4:3],,,
{4},,,2,
{4}3,4,5,6,
{4}7,1/8,,7,
{4}6,5,4,3,
{8}2,,1/8,,,,7/8,7/8,
{4},7<4[4:3]/8>3[4:3],,,
{8},,,,,,1/2,1/2,
{4},1<6[4:3]/2>5[4:3],,,
{4},,,4xh[8:3]/5xh[8:3],
{2},4xh[4:3]/6xh[4:3],
{8},,,,,3xh[8:5]/5xh[8:5],,,
{8},,,,,C1fh[1:3],,,
{1},
{1},
{1},
{4},,,1b,
{4},,,1/2,
{4}1/2,3,4,5,
{4}6,5/6,,7/8,
{4}7/8,6,5,4,
{4}3,3/4,,1/2,
{4}1/2,3,4,5,
{4}6,5/6,,7h[2:1]/8h[2:1],
{8},,,,,5b/6b,,5b/6b,
{8},3b/4b,,3b/4b,,1h[8:5]/2h[8:5],,,
{8},,,,,3b/4b,,3b/4b,
{8},5b/6b,,5b/6b,,7h[8:5]/8h[8:5],,,
{4},,,1h[2:1]/8h[2:1],
{8},,,,,,2,2,
{8}2,,3,3,3,,4,4,
{8}4,,5/6,,,,7,7,
{8}7,,6,6,6,,5,5,
{4}5,3b/4b,,2,
{4}2,1/2,1/2,3,
{4}3,4/5,4/5,6,
{4}6,7/8,7/8,5/6,
{4}5/6,4/7,,7,
{4}7,7/8,7/8,6,
{4}6,4/5,4/5,3,
{4}3,1/2,1/2,3/4,
{8}3/4,,2/5,,,,1,1,
{8}1,,2,2,2,,3,3,
{8}3,,4,4,4,,5,5,
{8}5,,6,6,6,,7,7,
{8}7,,8,8,8,,1/8,,
{4}2/7,3/6,4h[2:1]/5h[2:1],,
{16},,,,4x/5x,,,4x/5x,,,4x/5x,,,,,,
{1}3xh[4:11]/6xh[4:11],
{1},
{1},
{16},,,,4bx/5bx,,,4bx/5bx,,,4bx/5bx,,,,,,
{1},
{1},
{1},
E
&inote_4=
(190){1},
{16}5x/7x,,,5x/7x,,,4x/6x,,,,3x/5x,,,,2xh[8:3]/4xh[8:3],,
{8},,,1x-5[8:1],,6bx,,,
{16}4h[4:3]/7h[4:3],,,,,,,,,,,,,,,5x>2[16:5]/8x<5[16:5],
{1},
{16}5h[16:3],,,6h[16:3],,,7,,4h[16:3],,,3h[16:3],,,2,,
{16}6h[16:3],,,7h[16:3],,,8,,1,,,,2/4,,,,
{4}3/5,4/6,5/7,6x/8x,
{2}3bx/1bx-7[8:1],4bx/1bx-6[8:1],
{4}5bxh[2:1]/1bx>6[4:1],,,6bx/8bx-2[8:1],
{4},5bx/8bx-3[8:1],,4bxh[1:1]/8bx<3[4:1]p2[2:1],
{1},
{4}1/8,3bx/1bx-7[8:1],,4bx/1bx-6[8:1],
{4},5bxh[2:1]/1bx>6[4:1],,,
{8}7/8,,1,1,1,1h[2:1],,4,
{8},5,,6bx/5bx-1[8:1],,,8,8,
{8}8,8,,1/6,,1/6,,8xw4b[8:3],
{4},,,E8,
{4}E7,,B4/B5,E2,
{4}E3,,B4/B5,E8,
{4}E7,,E3/E4,E6/E7,
{8},,C1,,,,8,8,
{16}2,,2,,E1,B8,B7,E7,A6,,,,1,,1,,
{16}7,,7,,E1,B1,B2,E3,A3,,,,7p3[4:1],,,,
{8},,2q6[4:1],,,,7,8,
{8},7-4[8:1]/8-4[8:1],,,,,3h[4:3],,
{16},,,,A4,B4,C1,B8,A8,,,,2h[4:3],,,,
{16},,,,A1,B1,C1,B5,A5,,,,6h[4:3],,,,
{16},,,,A5,B5,C1,B1,A1,,,,7h[4:3],,,,
{16},,,,A8,B8,C1,B4,A4,,,,6x,,,,
{8}7,,8h[8:3],,4,4,,5/6,
{8},7/8,,1/2,,3xh[4:1]/4x,,6,
{8},5x/7xh[4:1],,2,,1xh[2:1]/4x,,8,
{8}8,8<4[4:1],,,,,3x,3,
{8}4,4,5,5,6,,5/7h[4:3],,
{16}E8,B7,,,D1,E1,,,E2,B2,,,6x,,6,,
{8}5,5,4,4,3,3,,2x/4x,
{16},2x/4x,,,3x/5x,,,3x/5x,,,4x/6x,,7,,7,,
{16}4,,4,,E1,E8,E7,E6,E5,,,,1,,1,,
{16}7,,7,,E1,E2,E3,E4,E5,,,,6,,6,,
{8}1,1,4,4,4h[4:1],,3,3,
{8}8,8,5,5,5h[4:1],,6,,
{8}1b/8b,,6,5,4,3,2,,
{8}7h[8:5]/8,,6,,8,8,,,
{8}1b/8b,,3,4,5,6,7,,
{4}1/2h[2:1],3,1,6b>1[4:3],
{4},8,,7,
{4},2/7,3/6,4x/5xh[4:1],
{8}6,7-3[8:1],,,,1-5[8:1],,,
{8},8-3[8:1],,,,2x/1x-6[8:1],,,
{8},7x/8x-3[8:1],,,,1x-6[8:1]/2x-5[8:1],,,
{8},7x-4[8:1]/8x-3[8:1],,,,,1bx/6bx,6bxh[8:11]/1bx<7[8:1]-3[8:1]<5[8:7],
{1},
{8},,,7b,,,3bx/8bx,3bxh[8:11]/8bx>2[8:1]-6[8:1]>4[8:7],
{1},
{8},,,2b,,,3bx/6bx,,
{8}5/6,6,5/6,6,5/6,6,6/5-3[8:1],,
{4}8,6/5-3[8:1],8,3bx/6bx,
{8}3/4,3,3/4,3,3/4,3,3/4-6[8:1],,
{8}1,,5/6,,4bx/8bx,,1x/2x,1x/2x,
{8}8,8,5,5,6>3[8:1],,7>2[8:1],,
{8}5,,1x/5x,1bx/5bx,1bx/5bx,,7x/8x,7x/8x,
{8}1,1,4,4,3<6[8:1],,2<7[8:1],,
{8}4,,5,4,3,2,1bx-5[8:1],,
{8}7,7-3[8:1],,8,8xh[2:1],,,,
{8}1,,2/7,,3/4,3h[8:3]/4,,2,
{8}2,,6/7,,1/8,1xh[8:7]/8x,,,
{4},3h[4:3],,5h[4:3],
{16},,,,7h[4:1],,,,,,2b/7b,,,,1,8,
{16}1,8,1,8,1,8,1,8,1,8,1xpp4[8:7]*qq6[8:7],,,,,,
{1},
{8},,,,,1b/5b,,7,
{8}7-3[8:1],,2-8[8:1],,6,,4,5,
{8}6,7,8,7,6,5,,1/2,
{8}1/2-6[8:1],,7-1[8:1],,3,,5,4,
{8}3,2,1,8,7,6b,,1/3,
{8}1/3-7[8:1],,8-2[8:1],,4,,5,4,
{8}3,2,1,2,3,4,,6/8,
{8}8/6-2[8:1],,1,,8,8x,,5x/7x,
{8},2x/4x,,3x/8x,,6bx/1bx>6[8:3],,,
{8},,8,,1/7,1x/7x,,2x/4x,
{8},5x/7x,,1x/6x,,3bx/8bx,,4/6,
{4}4h[2:1]/6,3,5h[2:1],6,
{8}4h[2:1],,3,,1,1b,,7,
{8}6,5,4,3,2,1,8,7,
{8}6,5,4,3,2,1x/6x,,3/4,
{8}3/4,,5/7,,1/6,6xh[8:7]/1x>8b[8:5],,,
{8},,,,,1bx/6bx,,2,
{4}2h[4:1],3h[4:1],4h[4:1],5h[4:1],
{8}6h[4:1],,7,,1bx/7bx,1bx/7bx,,7,
{4}7h[4:1],6h[4:1],5h[4:1],4h[4:1],
{8}3h[4:1],,2bx,2bx,2bx,1bx/8bx,,7,
{4}7-1[8:1],3-5[8:1],7-1[8:1],3-5[8:1],
{8}7-1[8:1],,3,,1bx/5bx,1bx/5bx,,2,
{4}2-5[8:1],6-1[8:1],2-5[8:1],6-1[8:1],
{8}2-5[8:1],,6,,4bx/8bx,4bx/8bx,,7,
{8}6,5,4,3,2,1b/6b,,2,
{8}3,4,5,6,7,3b/8b,,1/2,
{4}1/2-5[8:1],7-4[8:1],1>6[8:3],8h[2:1],
{8},,7x,,1/6,1/6,,3/4,
{4}3/4,5/7-3[8:1],2-6[8:1],8-4[8:1],
{16}1x,,,,2x/5x,,,4x/7x,,,1x/5x,,,,,,
{1}2bh[4:7]/8b<1[1:1],
{4},,,C1h[2:3],
{1},
{4},1w5b[8:1],,,
{1},
{1},
{1},
E
&inote_5=(190)
{1},
{16}7/3,,6,5,,3,{4}4-8[8:1],1>4[8:1],8x/1,{16}6,5,{8}6,7-3[7:1],,7/1-5[8:1],,5/1,6bh[16:15],,,,,,,{16},Ch[16:7],,,,,,,E8,E7,{8}E6,,1,,8h[16:3]/2h[16:3],,,3,6h[16:3]/4h[16:3],,,7/5,7h[16:3]/5h[16:3],,,5,{16}6/4,6/4,,,5/3,5/3,,,{12}2,4,3,7,5,6,2,4,3,{16}6,5,6,5,6xbh[4:1],,,,5,7,5,7,5xbh[4:1],,,,8,5,8,5,8xbh[4:1],,,,6,7,6,7,6,7,6,,7/5xbh[4:1],,,,6,4,6,4,5xbh[4:1],,,,4,6,4,6,5xb,,,,4>1b[8:1],5,4,5,{4}4<7b[8:1],4-8b[8:1],4,7x/1x,5b/3bh[4:1],{16}4,2,4,2,3bh[4:1],,,,2,4,2,4,3bh[4:1],,,,4,2,4,2,5,3,5,3,{8}5h[4:1],,4>2[8:1],,6,7>1[8:1],,4,4,3,2,{16}6xh[8:1]/2xh[8:1],,,6x/2x,,,{8}1<7[8:1],,3,{4}4>2[8:1],6h[4:1],1q5-7-2[4:3],5b,,,
{16},,A3,E4,E5,A5,{4},,A6/D6,{16}A3,E4,E5,A5,{4},,A6/D6,{16}A3,E4,E5,A5,{4},,D7/A6/B5,E8/B7/4h[4:3],,,5,{16}A4,E5,E6,A6,E4,B4,B6,E7,A3,B3,B7,A7,C/B1,,,,A5,E5,E4,A3,E6,B5,B3,E3,A6,B6,B2,A2,{8}C/B8,,1h[16:3],,,5/3,5h[16:3]/3h[16:3],,,4,4,6,6,8p3[4:1]/7<4[4:1],,,,,{16}2,4,2,4h[16:9],{4},5h[2:1],,{16}6,5,6,5h[16:9],{4},7h[2:1],,{16}8,7,8,7h[16:13],{4},1,1>6[2:1],,,,{8}8,8,1x/2x,,6/5,,7/8,,4/3,{4}4-6[8:1]/3,2-4[8:1],4,5-7[8:1],4h[4:1],{8}8-4[8:1],8,1h[4:1],,3-7[8:1],3,2x,,8,7,6,,8/2,8/2,,
{16}8,7,6,5,1,2,3,4,6,7,8,1,4,3,2,1,7,6,5h[8:1],,1,2,3h[8:1],,8,7,6h[8:1],,4,5,4,5,4,3,2,1,5,6,7,8,2,3,4,5,1,8,7,6h[16:3],,,5,,,7/8,,,4/3,,,1/2,,,6/4,,6/4,3,2,1,7,6,5,4,8,1,2,3,8,7,6,5,1,2,3,4,1/5,8/4,7/3,6/2,5/1b,,,,7,8,7,8,{4}6>3[4:1],3>8[4:1],8-6-4[4:1],4>1[4:1],1<6[4:1],6>3[4:1],3,2w6[8:1],,8/4,{8}7h[8:1],3,7h[8:1],4h[16:1],7h[8:1],4h[16:1],7h[8:1],4h[16:1],7h[8:1],5,7h[8:1]/5-1[8:1],,6,,8/2,,3h[8:1],5,3h[8:1],4h[16:1],3h[8:1],4h[16:1],3h[8:1],4h[16:1],3h[8:1],4,4/3,{24}4,3,5,{4}2<7[4:1],{16},,,6,6>3[4:1],,,,,,,2,{4}2h[4:1],4,{24}3,,4,3,5,,{8}6,6,6xh[4:1],,5,7-5-1[4:1],,2,1,8-4-2[4:1],,7,6,5-7-3[4:1],,2,3,4-8-2[4:1],,5,6,7-1-5[4:1],,4,5,6-2-4[4:1],,7,8,1-5[8:1],,4,4,,
6xb/1xb,6xbh[4:7]/1xb-5-7-4-2[4:3],,,,,,,,,,,,1b,,5,8xb/3xb,8xb-4-2-5-7[4:3]/3xbh[4:7],,,,,,,,,,,,8b,,5,5xb/4xb,,6h[16:1]/5h[16:1],6/5,6h[16:1]/5h[16:1],6/5,7h[16:1]/4h[16:1],7/4,6h[16:1]/5h[16:1],6/5,7/4,,6/5-3[8:1],,7/5,,5xb/2xb,,{24}3,5,3,5,,,6,4,6,4,,,3,5,3,5,,,6,4,6,4,,,{4}8<5[8:1]/2>5[8:1],8x/2x,E7/B6/B5/B4/E4,8b/1b,{16}1,8,1,8h[8:1],,2h[8:1],,1,2,,7/3,,8,2,8,2,1,2,1,2,3,2,4,2,5,2,6,2,7b,,,,6,4,6,4h[8:1],,7h[8:1],,8,7,,6/2,,1,7,1,7,8,7,8,7,6,7,5,7,4,7,3,7,{8}2xb-5[8:1],,6,6q1[2:1],,4,4xh[2:1],,,,8h[4:1],,6,,7/5,7/5,,5/3,5/3h[4:1],,2,4,6,8xh[2:1],,6,6,6,7h[8:3]/5,5,5,5,6h[8:3]/4,4,4,4,5/4,6/3,7/2,{16}1,8,1,8,1,8,2,8,1,8,1,7,1-6[8:1],8,2,8,1b,,{2},,7h[4:1]/1h[4:1],{8}6/2,6-3b[8:1]/2-7b[8:1],,6b/2b,,
{16}1,6,2,5,3,4,1,8,1,8,2,5,3,4,1,7,1,7,3,5,4,6,2,8,2,8,3,7,5b,,,,6,6,4,3,2,1,5/8,6,7,8,1/5,4,3,2,8/1,7,6,5,4-8[8:1]/3,2,1-5[8:1],,4,,1,,,,6b/4b,,,,2x,1,3,8,4,7,5,6,5,6,3,7,2,6,1,5,1,5,2,4,2,4,3,3,5,5,4,4,6h[4:1],,,,{32}2x,3,4,5,2x,3,4,5,2x,3,4,5,1x,2,3,4,1x,2,3,4,1x,2,3,4,{4}5xb-1[8:1],7-4[8:1],7/8,2>8[4:1],5h[4:1]/2,{24}1,2,3,{8}4b-8-2-5[4:1],,5,,7,7-1[8:1],,7-4[8:1]/2-6[8:1],,7/2,,5/2,,6,1,{32}1,2,3,4,E2,E3,E4,E5,,,,,7,,,,7,6,5,4,E7,E6,E5,E4,{16},,1x/2x,,3,8,3,8,4,7,5,6,4,7,4,7,5xbh[4:1],,,,7,7,5,5,6,6,3,3,4,4,2,2,8,8,1,7,2,6,3,5,4,6,3,7,2,8,1,,8xb/2xbh[4:1],,,,{32}7x,6,5,4,7x,6,5,4,7x,6,5,4,8x,7,6,5,8x,7,6,5,8x,7,6,5,{8}4xb>1[8:1],,7,7-4[8:1],,7/8,1<6[8:1],,
5bh[8:11
]/1b,{4},,,6,,6,8,{8}7b/1b,6b/2bh[8:11],,,{2}4,4,{8}4,,5b/3b,6b/2b,7b/1b,7bh[4:1]/1b,,5,{4}5-3[8:1],7-5[8:1]/5,7-3[8:1]/5,7/1,4-8[8:1]/2-5[8:1],4/2,{8}7xb/1xb,7xb/1xbh[4:1],,4,{4}4-6[8:1],4/2-4[8:1],4/2-6[8:1],8/2,7-4[8:1]/5-1[8:1],7/5,{8}8xb/2xb,8xb/2xbh[4:1],,{16}8,8,3,3,4,4,5,5,7,7,8,8,7xbh[4:1]/1xb,,,,1,1,6,6,5,5,4,4,2,2,1,1,8xb/2xbh[4:1],,,,{32}7x,6,5,4,7x,6,5,4,7x,6,5,4,8x,7,6,5,8x,7,6,5,8x,7,6,5,8x,1,2,3,8x,1,2,3,8x,1,2,3,8x,7,6,5,8x,7,6,5,{16}4xb,,,,6/2,,6/2,,3,5,3,5,4,5,4,5,{24}4,3,2,1/8,7,6,5/3,2,1,7/8,6,5,{32}4/1,8,7,6,1/5,8/4,7/3,6/2,{16}5b/1b,,,,8b/2b,,,6b/4b,,,7bh[8:27]/Ch[8:3],,,,,,{24}B8/B1,E1,A8/D1f/A1,{2},,,{16}3,2,{2}1h[4:5],,,{16}8xb,3,5,1xbw5b[8:1],6,4,A1f,";

    private SimaiFile file;

    private static Stream GenerateStreamFromString(string s)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(s);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        file = new SimaiFile(GenerateStreamFromString(CHART_TEXT));
        var chart = SimaiConvert.Deserialize(file.GetValue("inote_5"));

        GD.Print($"Center => {GetRect()}");

        var x = 0;
        foreach (var noteCollection in chart.NoteCollections)
        {
            var y = 0;

            foreach (var note in noteCollection.ToArray())
            {
                GD.Print(
                    $"Note collection => {x}, Note index => {y}, Note => {note.type}, Slide Segments Count => {note.slidePaths.Count}, Slide morph => {note.slideMorph}");

                if (note.type == NoteType.Slide)
                    foreach (var slideGenerator in from slidePath in note.slidePaths
                             from slideSegment in slidePath.segments
                             select SlideUtility.MakeSlideGenerator(slideSegment))
                        GD.Print($"Slide Generator => {slideGenerator}, Is null => {slideGenerator == null}");

                y++;
            }

            x++;
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
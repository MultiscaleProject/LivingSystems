import java.util.Map;
import java.util.HashMap;

PGraphics pg;
Extract xtr = new Extract();
ArrayList<ArrayList<String>> networks;
Network net;
Network neet0;
ArrayList<String> networkstr;

String file_name = "evolucion";
float desired_size = 0.2; // Space for the network
int transparency = 240;
int wait = 1; // Time steps displaying the same network
int generation = 0 * wait;
boolean save_frames = false;
int cleanConnetions = true ? 1 : 2;
int totalNodes;

int selectedNode = -1;
boolean isDragging = false;

void setup() {
  String fN = file_name + ".txt";
  size(550, 800);
  pg = createGraphics(width, height, JAVA2D);

  networks = xtr.Networks(fN);
  networkstr = networks.get(0);
  totalNodes = networkstr.size() - 4; // Calculate total nodes from first network
  neet0 = new Network(totalNodes);
  neet0.Str2Network(networkstr);
  for (int i = 0; i < 2000; i++) neet0.CalculatePosition();
  net = neet0;
}

void draw() {
  background(255);
  pg.beginDraw();
  // pg.background(48, 56, 65);
  // pg.background(255, 255);
  pg.clear();

  networkstr = networks.get(generation / wait);
  if (generation % wait == 0 && frameCount/wait < networks.size() - 1) {
    Network nextGen = new Network(totalNodes);
    nextGen.Str2Network2(networkstr, neet0);
    
    for (int i = 0; i < 1; i++) nextGen.CalculatePosition();  

    if (isDragging && selectedNode != -1) {
      nextGen.updateNodePosition(selectedNode, mouseX, mouseY);
    }
    
    net.transferAnimatedCircles(nextGen);
    
    neet0 = nextGen;
    net = nextGen;
  }
  for (int i = 0; i < 1; i++) net.CalculatePosition();
  net.Draw(0.10, 0.10, 0.90, 0.95);
  pg.endDraw();
  image(pg, 0, 0);

  String strcleanConnetions = (cleanConnetions == 1) ? "/cleanConnetions/" : "/allConnections";
  if (save_frames) pg.save(file_name + strcleanConnetions + "/Network" + nf(frameCount, 6) + ".png");
  if (++generation / wait >= networks.size()) exit();
}

void mousePressed() {
  selectedNode = net.getNodeAtPosition(mouseX, mouseY);
  if (selectedNode != -1) {
    isDragging = true;
  }
}

void mouseReleased() {
  isDragging = false;
}

class Network {
  ArrayList<Node> nodes = new ArrayList<Node>();
  ArrayList<Integer> chosen = new ArrayList<Integer>();
  HashMap<String, AnimatedCircle> animatedCircles = new HashMap<String, AnimatedCircle>();

  float force = 0.005;
  int ninputs;
  String gentxt = "";
  int output;

  Network(int totalNodes) {
    for (int i = 0; i < totalNodes; i++) {
      nodes.add(new Node(i));
    }
  }

  void updateAnimatedCircles() {
    for (AnimatedCircle circle : animatedCircles.values()) {
      circle.update();
    }
  }

  int getNodeAtPosition(float mx, float my) {
    for (int i = 0; i < nodes.size(); i++) {
      if (chosen.contains(i)) {
        Node n = nodes.get(i);
        float x = Reposition(n.vP.x, 0.05, 0.95, width);
        float y = Reposition(n.vP.y, 0.10, 0.95, height);
        float size = map(n.outputs.size(), 0, 6, 35, 50);
        if (dist(mx, my, x, y) < size / 2) {
          return i;
        }
      }
    }
    return -1;
  }

  void updateNodePosition(int nodeIndex, float mx, float my) {
    if (chosen.contains(nodeIndex)) {
      Node n = nodes.get(nodeIndex);
      n.vP.x = map(mx, 0.05 * width, 0.95 * width, 0, 1);
      n.vP.y = map(my, 0.10 * height, 0.95 * height, 0, 1);
      n.vP.x = constrain(n.vP.x, 0, 1);
      n.vP.y = constrain(n.vP.y, 0, 1);
      n.isDragged = true;
    }
  }

  void Chosen(ArrayList<String> net_str) {
    chosen.clear();
    int lineaNodosAConsiderar = 3 - cleanConnetions;
    for (String e_str : split(net_str.get(net_str.size() - lineaNodosAConsiderar), ',')) {
        chosen.add(int(e_str));
    }
  }

  void InputsOutputsColors() {
    nodes.get(output).Color(250, 0, 0);
    // for (int i = 0; i < ninputs/2; i++) nodes.get(i).Color(60 - i*5, 185 - i*50, 200 + i*15);
    // for (int i = ninputs/2; i < ninputs; i++) nodes.get(i).Color(200 + (i-ninputs/2)*15, (i-ninputs/2)*55, 194 + 22);
    for (int i = 0; i < ninputs/2; i++) nodes.get(i).Color(100, 150, 200);
    for (int i = ninputs/2; i < ninputs; i++) nodes.get(i).Color(230, 180, 230);
  }

  void CountInputs(ArrayList<String> net_str) {
    ninputs = 0;
    for (int i = 2; i < net_str.size() - 2; i++) {
      if (split(net_str.get(i), '|')[2].equals("")) ninputs++;
    }
  }

  void calculateAverageColors() {
    for (int i = 0; i < nodes.size(); i++) {
      Node n = nodes.get(i);
      if (i >= ninputs && i != output) {
        int totalR = 0, totalG = 0, totalB = 0;
        int count = 0;
        for (int input : n.inputs) {
          if (chosen.contains(input)) {
            Node inputNode = nodes.get(input);
            totalR += inputNode.r;
            totalG += inputNode.g;
            totalB += inputNode.b;
            count++;
          }
        }
        if (count > 0) {
          n.r = totalR / count;
          n.g = totalG / count;
          n.b = totalB / count;
        } 
        if (n.r + n.g + n.b < 85) {
          n.r = 225;
          n.g = 225;
          n.b = 225;
        }
      }
    }
  }

  void Str2Network(ArrayList<String> net_str) {
    gentxt = net_str.get(0);
    output = int(split(net_str.get(1), ' ')[2]);
    CountInputs(net_str);
    Chosen(net_str);

    for (int i = 2; i < net_str.size()-2; i++) {
      String[] inputs = split(split(net_str.get(i), '|')[1], ',');
      Node n = nodes.get(i - 2);
      n.index = str(i - 2);
      n.strBF = split(net_str.get(i), '|')[2];
      n.Color(0, 0, 0);
      n.inputs.clear();

      // Add real inputs without conditions
      if (inputs.length > 0) n.inputs_real.add(int(inputs[0]));
      if (inputs.length > 1) n.inputs_real.add(int(inputs[1]));

      if (i-2 > ninputs-1 && chosen.contains(i - 2)) {
        if (cleanConnetions == 2 || (!n.strBF.equals("1010") && !n.strBF.equals("0101"))) {
          n.inputs.add(int(inputs[0]));
        }
        if (cleanConnetions == 2 || (!n.strBF.equals("0011") && !n.strBF.equals("1100"))) {
          n.inputs.add(int(inputs[1]));
        }
      }
    }
    InputsOutputsColors();
    FindOutputs();
    determineRegulationPaths();
    for (int i = 0; i < 10; i++) calculateAverageColors();
  }

  void Str2Network2(ArrayList<String> net_str, Network oldnet) {
    ArrayList<PVector> oldPositions = new ArrayList<PVector>();
    for (Node n : oldnet.nodes) {
      oldPositions.add(n.vP.copy());
    }
    
    Str2Network(net_str);
    
    for (int i = 0; i < nodes.size(); i++) {
      if (i < oldPositions.size()) {
        nodes.get(i).vP = oldPositions.get(i);
      }
    }
    
    force = oldnet.force;
    if (force > 0.03) force = 0.03;
    calculateAverageColors();
  }

  void FindOutputs() {
    for (Node n : nodes) n.outputs.clear();
    for (int i = 0; i < nodes.size(); i++) {
      for (int j = 0; j < nodes.size(); j++) {
        if (nodes.get(j).inputs.contains(i)) nodes.get(i).outputs.add(j);
      }
    }
  }

  void AdjustForce() {
    float minX, minY, maxX, maxY;
    minX = minY = 1.0;
    maxX = maxY = 0.0;
    for (int i = 0; i < nodes.size(); i++) {
      if (i >= ninputs && i != output) {
        Node n = nodes.get(i);
        if (minX > n.vP.x) minX = n.vP.x;
        if (minY > n.vP.y) minY = n.vP.y;
        if (maxX < n.vP.x) maxX = n.vP.x;
        if (maxY < n.vP.y) maxY = n.vP.y;
      }
    }
    float space = (maxX - minX) * (maxY - minY);
    force += (desired_size - space) / 500.0;
    force = constrain(force, 0.003, 0.04);
  }

  void CalculateRepulsiveForces() {
    for (int i : chosen) {
      Node v = nodes.get(i);
      v.svP = new PVector(0.5 - v.vP.x, 0.5 - v.vP.y);
      for (int j : chosen) {
        if (i != j) {
          Node u = nodes.get(j);
          PVector dif = PVector.sub(v.vP, u.vP);
          float d = dif.mag();
          d = max(d, 0.0001);
          float forceMagnitude = min(0.75*force / (d * d), 0.2);
          dif.normalize().mult(forceMagnitude);
          v.svP.add(dif);
        }
      }

      float wallForce = 0.05;
      float leftDist = v.vP.x;
      float rightDist = 1.0 - v.vP.x;
      float topDist = v.vP.y;
      float bottomDist = 1.0 - v.vP.y;

      if (leftDist < wallForce) v.svP.x += wallForce / (leftDist * leftDist);
      if (rightDist < wallForce) v.svP.x -= wallForce / (rightDist * rightDist);
      if (topDist < wallForce) v.svP.y += wallForce / (topDist * topDist);
      if (bottomDist < wallForce) v.svP.y -= wallForce / (bottomDist * bottomDist);
    }
  }

  void CalculateAttractiveForces() {
    for (int i : chosen) {
      Node v = nodes.get(i);
      for (int r : v.inputs) {
        if (chosen.contains(r)) {
          Node u = nodes.get(r);
          PVector dif = PVector.sub(v.vP, u.vP);
          float d = dif.mag();
          dif.mult(d);
          v.svP.sub(dif);
          u.svP.add(dif);
        }
      }
    }
  }

  void LimitMaximumDisplacement() {
    for (int i : chosen) {
      Node v = nodes.get(i);
      if (v.svP.x != 0) v.vP.x += 0.001 * v.svP.x / abs(v.svP.x);
      if (v.svP.y != 0) v.vP.y += 0.001 * v.svP.y / abs(v.svP.y);
      v.vP.x = constrain(v.vP.x, 0, 1);
      v.vP.y = constrain(v.vP.y, 0, 1);
      if (v.inputs.size() == 0) v.vP = new PVector(random(0.4, 0.6), 0.5);
    }
  }

  void AdjustInputsAndOutput() {
    float inputDelta = 0.20;
    for (int i = 0; i < ninputs/2; i++) {
      if (chosen.contains(i)) {
        nodes.get(i).vP.x = map(i, 0, ninputs/2, 0, inputDelta) + 0.05;
      }
    }
    for (int i = ninputs/2; i < ninputs; i++) {
      if (chosen.contains(i)) {
        nodes.get(i).vP.x = map(i, ninputs/2 -1, ninputs-1, 1 - inputDelta, 1) - 0.05;
      }
    }
    for (int i = 0; i < ninputs; i++) {
      if (chosen.contains(i)) {
        nodes.get(i).vP.y = 0.0;
      }
    }
    if (chosen.contains(output)) {
      nodes.get(output).vP.x = 0.5;
      nodes.get(output).vP.y = 1;
    }
  }

  void ApplyRegulationPathForces() {
    float lateralForce = 0.01;
    for (int i : chosen) {
      if (i >= ninputs) {
        Node n = nodes.get(i);
        if (n.hasPathToFirstInputs && !n.hasPathToSecondInputs) {
          n.svP.x -= lateralForce;
        } else if (!n.hasPathToFirstInputs && n.hasPathToSecondInputs) {
          n.svP.x += lateralForce;
        }
      }
    }
  }

  void CalculatePosition() {
    AdjustForce();
    CalculateRepulsiveForces();
    CalculateAttractiveForces();
    ApplyRegulationPathForces();
    LimitMaximumDisplacement();
    AdjustInputsAndOutput();
    ResetDraggedNodes();
  }

  void ResetDraggedNodes() {
    for (Node n : nodes) {
      if (n.isDragged) {
        n.svP.set(0, 0);
        n.isDragged = false;
      }
    }
  }

  void determineRegulationPaths() {
    for (Node n : nodes) {
      n.hasPathToFirstInputs = false;
      n.hasPathToSecondInputs = false;
      n.checked = false;
    }
    
    for (int i = ninputs; i < nodes.size(); i++) {
      if (chosen.contains(i)) {
        checkRegulationPath(i);
      }
    }
  }

  void checkRegulationPath(int startNodeIndex) {
    ArrayList<Integer> toCheck = new ArrayList<Integer>();
    toCheck.add(startNodeIndex);
    
    while (!toCheck.isEmpty()) {
      int nodeIndex = toCheck.remove(0);
      Node n = nodes.get(nodeIndex);
      
      if (n.checked) continue;
      n.checked = true;
      
      for (int input : n.inputs) {
        if (input < ninputs / 2) {
          n.hasPathToFirstInputs = true;
        } else if (input < ninputs) {
          n.hasPathToSecondInputs = true;
        } else if (chosen.contains(input)) {
          Node inputNode = nodes.get(input);
          if (!inputNode.checked) {
            toCheck.add(input);
          }
          n.hasPathToFirstInputs |= inputNode.hasPathToFirstInputs;
          n.hasPathToSecondInputs |= inputNode.hasPathToSecondInputs;
        }
        
        if (n.hasPathToFirstInputs && n.hasPathToSecondInputs) {
          break;
        }
      }
    }
  }

  float Reposition(float p, float p1, float p2, float widthheight) {
    return widthheight * (p1 + map(p, 0, 1, 0, p2 - p1));
  }

  void DrawRegulations(Node n, Node n_reg, float x1, float y1, 
  float x2, float y2, float tmhN1, float tmhN2, int regulatorIndex) {
    pg.stroke(n_reg.r, n_reg.g, n_reg.b, transparency);
    pg.strokeWeight(tmhN1 * 0.16);
    pg.noFill();

    float distance = dist(x1, y1, x2, y2);
    float curvature = map(distance, 0, width/2, 0.075, 0.15);

    float ctrlX1, ctrlY1, ctrlX2, ctrlY2;

    if (n == n_reg) {  // Caso de autorregulación
      float loopSize = tmhN1 * 1.5;
      ctrlX1 = x1 - loopSize;
      ctrlY1 = y1 - loopSize;
      ctrlX2 = x1 + loopSize;
      ctrlY2 = y1 - loopSize;
      
      pg.bezier(x1, y1, ctrlX1, ctrlY1, ctrlX2, ctrlY2, x1, y1);
    } else {
      PVector diff = new PVector(x2 - x1, y2 - y1);
      PVector perp = new PVector(-diff.y, diff.x);
      perp.normalize().mult(distance * curvature);
      
      // Ajustar la dirección de la curva según el regulador
      if (regulatorIndex == 0) {
        perp.mult(-1); // Curva hacia la izquierda para el primer regulador
      }

      ctrlX1 = x1 + diff.x * 0.3 + perp.x;
      ctrlY1 = y1 + diff.y * 0.3 + perp.y;
      ctrlX2 = x1 + diff.x * 0.7 + perp.x;
      ctrlY2 = y1 + diff.y * 0.7 + perp.y;

      pg.bezier(x1, y1, ctrlX1, ctrlY1, ctrlX2, ctrlY2, x2, y2);
    }
    
    String key = n_reg.index + "->" + n.index;
    if (animatedCircles.containsKey(key)) {
      AnimatedCircle circle = animatedCircles.get(key);
      circle.updatePoints(x1, y1, x2, y2, ctrlX1, ctrlY1, ctrlX2, ctrlY2);
    } else {
      AnimatedCircle circle = new AnimatedCircle(x1, y1, x2, y2, ctrlX1, ctrlY1, ctrlX2, ctrlY2, n_reg.r, n_reg.g, n_reg.b);
      animatedCircles.put(key, circle);
    }
  }

  void Draw(float px1, float py1, float px2, float py2) {
    pg.textSize(22);
    for (Node n1 : nodes) {
      float x1 = Reposition(n1.vP.x, px1, px2, width);
      float y1 = Reposition(n1.vP.y, py1, py2, height);
      float tmhN1 = map(n1.outputs.size(), 1, 6, 40, 46);

      pg.noStroke();
      
      int nodeIndex = nodes.indexOf(n1);
      if (nodeIndex < ninputs || nodeIndex == output) {
        pg.fill(n1.r, n1.g, n1.b, 255);
      } else {
        pg.fill(n1.r, n1.g, n1.b, transparency);
      }

      if (chosen.contains(nodeIndex)) {
        
          if (nodeIndex < ninputs || nodeIndex == output) {
            pg.fill(n1.r, n1.g, n1.b, 255);
            pg.ellipse(x1, y1, tmhN1, tmhN1);  
          } else {
            for (float xj = 1.6; xj >= 0.2; xj = xj-0.2) {
              pg.fill(n1.r, n1.g, n1.b, map(xj, 1.4, 0.2, 20, 255));
              pg.ellipse(x1, y1, xj*tmhN1, xj*tmhN1);  
            }
          }
          
      }

      pg.strokeWeight(1.5);

      for (int i = 0; i < n1.inputs.size(); i++) {
        int ind = n1.inputs.get(i);
        Node n2 = nodes.get(ind);
        float x2 = Reposition(n2.vP.x, px1, px2, width);
        float y2 = Reposition(n2.vP.y, py1, py2, height);
        float tmhN2 = map(n2.outputs.size(), 0, 7, 25, 50);

        if (chosen.contains(ind)) {
          DrawRegulations(n1, n2, x1 + (tmhN1/2)*(2*i-1), y1, x2, y2, tmhN1/3, tmhN2/2, i);
        }
      }
    }

    updateAnimatedCircles();

    for (AnimatedCircle circle : animatedCircles.values()) {
      circle.display(pg);
    }

    for (Node n1 : nodes) {
      float x1 = Reposition(n1.vP.x, px1, px2, width);
      float y1 = Reposition(n1.vP.y, py1, py2, height);
      pg.noStroke();
      pg.fill(n1.r, n1.g, n1.b, transparency);
      if (chosen.contains(nodes.indexOf(n1))) {
        pg.fill(95, 95, 245);
        pg.fill(0);
        pg.text(n1.index, x1-7, y1+5);
        pg.fill(0, 155, 0);
        pg.text(n1.strBF, x1, y1-20);
      }
    }

    pg.textSize(11);
    pg.fill(10);
    // pg.text(gentxt, width * 0.055, height * 0.04);
  }

  void transferAnimatedCircles(Network nextGen) {
    for (Map.Entry<String, AnimatedCircle> entry : animatedCircles.entrySet()) {
      if (nextGen.animatedCircles.containsKey(entry.getKey())) {
        nextGen.animatedCircles.get(entry.getKey()).t = entry.getValue().t;
      }
    }
  }
}

class AnimatedCircle {
  float startX, startY, endX, endY;
  float ctrlX1, ctrlY1, ctrlX2, ctrlY2;
  float t = 0.95;
  int r, g, b;
  
  AnimatedCircle(float sX, float sY, float eX, float eY, float cX1, float cY1, float cX2, float cY2, int red, int green, int blue) {
    updatePoints(sX, sY, eX, eY, cX1, cY1, cX2, cY2);
    r = red; g = green; b = blue;
  }
  
  void updatePoints(float sX, float sY, float eX, float eY, float cX1, float cY1, float cX2, float cY2) {
    startX = eX; startY = eY; endX = sX; endY = sY;
    ctrlX1 = cX2; ctrlY1 = cY2; ctrlX2 = cX1; ctrlY2 = cY1;
  }
  
  void update() {
    t += 0.002;
    if (t > 1) t = 0.95;
  }
  
  void display(PGraphics pg) {
    float x = bezierPoint(startX, ctrlX1, ctrlX2, endX, t);
    float y = bezierPoint(startY, ctrlY1, ctrlY2, endY, t);
    pg.noStroke();
    pg.fill(r, g, b, 255);
    pg.ellipse(x, y, 8, 8);
  }
}

class Node {
  ArrayList<Integer> inputs = new ArrayList<Integer>();
  ArrayList<Integer> inputs_real = new ArrayList<Integer>(); // New list for real inputs
  ArrayList<Integer> outputs = new ArrayList<Integer>();
  String index = "";
  String strBF = "";

  PVector vP;
  PVector svP = new PVector(0, 0);
  int r = int(255);
  int g = int(255);
  int b = int(255);

  boolean hasPathToFirstInputs = false;
  boolean hasPathToSecondInputs = false;
  boolean checked = false;

  boolean isDragged = false;

  Node(int i) {
    vP = new PVector(random(0.4, 0.6), random(0.4, 0.6));
    index = str(i);
  }

  void Color(int r_, int g_, int b_) {
    r = r_;
    g = g_;
    b = b_;
  }
}

class Extract {
  ArrayList<ArrayList<String>> Networks(String fN) {
    String[] data = loadStrings(fN);
    ArrayList<ArrayList<String>> networks = new ArrayList<ArrayList<String>>();
    ArrayList<String> l = new ArrayList<String>();
    for (String s : data) {
      if (s.equals("")) {
        networks.add(l);
        l = new ArrayList<String>();
      } else l.add(s);
    }
    networks.add(l);
    return networks;
  }
}






**García Sierra**

**Práctica Tela**

**Animación 3D**

**y Juan Carlos**

() crea los

En la función FixedUpdate() decide si calcularemos las físicas mediante Euler Explícito u

Implícito y llama a los métodos de cálculo de físicas cuantas veces como “substeps” tengamos.

En los métodos de Euler se calcularán las físicas mediante las funciones de los nodos y muelles

y la fuerza del viento.

Las demás clases heredan atributos de la clase MassSpringCloth para que estas puedan ser

modificadas desde el editor de Unity.

En la función OnDrawGizmos() pinta los nodos y muelles del tetraedro y los nodos de la malla

high poly

**-Node:**

La clase nodo es un Script muy sencillo que contiene un constructor que asigna las variables de

la clase MassSpringCloth y un método ComputeForces() que calcula la fuerza de los nodos

(masa\*gravedad) y la fuerza de amortiguamiento.

La clase MassSpringCloth usa esta clase para crear objetos que asignará a una lista.

La clase Spring usa la clase Node para calcular las posiciones y las físicas.

La clase Edge usa estos nodos para ordenar una lista según el orden del índice de los nodos.

La clase Fixer usa objetos tipo Node para cambiar su variable isFixed.

La clase Parser crea objetos de tipo de Node para asignárselos a los tetraedros.

**-Spring:**

La clase nodo es un Script muy sencillo que contiene un constructor que asigna las variables de

la clase MassSpringCloth, un método ComputeForces() que calcula la fuerza de los muelles de

Tracción y calcula la fuerza de amortiguamiento (rotación y deformación de los muelles). Tiene

un método UpdateLength() que calcula el tamaño del muelle según sus nodos para el cálculo

de físicas.

El método ComputeForces() calcula la densidad de rígidez.

La clase MassSpringCloth crea objetos de tipo Spring que asignará a una lista.

La clase Parser crea objetos de tipo Spring para asignárselos a los tetraedros

**-Fixer:**

Esta clase calcula que nodos de los tetraedros que están dentro de un Collider para cambiar su

variable isFixed, por lo que dichos nodos no se moverán.

La clase MassSpringCloth crea un objeto tipo Fixer para anclar los nodos.

**FixerMovement:**

Este Script está asociado al gameObject que actúa como fixer. Según este gameObject se

mueva, moverá los nodos fijados en consecuencia.





**Parser:**

La clase parser recibe los ficheros txt con la información de nodos y muelles de los tetraedros y

los añade a una lista.

La clase ElasticSolid accede a la lista de tetraedros y la clase Parser añade los nodos y muelles

leídos a la clase ElasticSolid.

**Tetraedro:**

La clase Tetraedro contiene 4 nodos y las normales del tetraedro. El método isInside() calcula

si los nodos de la malla están dentro del tetraedro o no, el método calculateBar() calcula las

coordenadas baricéntricas de un nodo con respecto a los nodos del tetraedro. El método

calculatePos() calcula la posición de un nodo dentro del Tetraedro. El método calculateMass()

calcula la masa de los nodos del tetraedro.

La clase Parser crea objetos tipo Tetraedro.

La clase ElasticSolid calcula los nodos dentro del Tetraedro, pinta los tetraedros y calcula las

posiciones de los Tetraedros.

**Instrucciones de simulación:**

Para hacer la simulación se recomienda acceder al GameObject pre-establecido con los valores

ya asignados, también se puede arrastrar un prefab “GameObject” si no hay ninguno en la

escena.

Todos los parámetros son modificables, pero puede que algunos cambios en estos no sean

estables para la simulación.

Los parámetros por defecto (recomendados) son los siguientes:

Si el objeto se cae, eliminar el componente Fixer y volverlo a asignar.

Se entrega una carpeta con el proyecto entero y un .unitypackage ,elegir el que mejor

convenga.


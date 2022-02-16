

**García Sierra**

**Práctica Tela**

**Animación 3D**

**y Juan Carlos**

s y los

la función

FixedUpdate() decide si calcularemos las físicas mediante Euler Explícito u Implícito y llama a

los métodos de cálculo de físicas cuantas veces como “substeps” tengamos. En los métodos de

Euler se calcularán las físicas mediante las funciones de los nodos y muelles y la fuerza del

viento.

Las demás clases heredan atributos de la clase MassSpringCloth para que estas puedan ser

modificadas desde el editor de Unity.

**-Node:**

La clase nodo es un Script muy sencillo que contiene un constructor que asigna las variables de

la clase MassSpringCloth y un método ComputeForces() que calcula la fuerza de los nodos

(masa\*gravedad) y la fuerza de amortiguamiento.

La clase MassSpringCloth usa esta clase para crear objetos que asignará a una lista.

La clase Spring usa la clase Node para calcular las posiciones y las físicas.

La clase Edge usa estos nodos para ordenar una lista según el orden del índice de los nodos.

La clase Fixer usa objetos tipo Node para cambiar su variable isFixed.

**-Spring:**

La clase nodo es un Script muy sencillo que contiene un constructor que asigna las variables de

la clase MassSpringCloth, un método ComputeForces() que calcula la fuerza de los muelles

según sean de Flexión o de Tracción y calcula la fuerza de amortiguamiento (rotación y

deformación de los muelles). Por último tiene un método UpdateLength() que calcula el

tamaño del muelle según sus nodos para el cálculo de físicas.

La clase MassSpringCloth crea objetos de tipo Spring que asignará a una lista.

**-Fixer:**

Esta clase calcula que nodos están dentro de un Collider para cambiar su variable isFixed, por

lo que dichos nodos no se moverán

La clase MassSpringCloth crea un objeto tipo Fixer para anclar los nodos.

**FixerMovement:**

Este Script está asociado al gameObject que actúa como fixer. Según este gameObject se

mueva, moverá los nodos fijados en consecuencia.

**-Edge:**

La clase Edge solo contiene 3 nodos y un constructor el cual ordena los dos primeros nodos.

La Clase MassSpringCloth crea objetos tipo Edge los cuales añade a una lista para

posteriormente ordenarlos y crear nodos de tracción o de flexión con el tercer nodo de este

objeto según ya hayan sido creados o no.





**-Comparer:**

La clase Comparer hereda del interfaz IComparer, el cual se aplica a la clase Edge para ordenar

una lista con objetos de esta clase Edge.

La clase MassSpringCloth usa la clase Comparer para busca aristas duplicadas poniéndolas en

una lista, ordenándola, y después mirando si las aristas consecutivas son iguales.

**Instrucciones de simulación:**

La dirección del archivo “.unity” es:

**AlvaroGarciaSierra/Practica\_Tela/Assets/\_ssets/PracticaTela.unity**

Para hacer la simulación arrastrar un prefab “Plano\_Y\_Fixer\_Prefab” si no hay ninguno en la

escena.

Todos estos parámetros son modificables, pero puede que algunos cambios en estos no sean

estables para la simulación.

Los parámetros por defecto (recomendados) son los siguientes:

parar la ejecución de

plementación,

odos

s tracción

s flexión

nto nodos

nto rotación

nto deformación

viento

fricción del viento.


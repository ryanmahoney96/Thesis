
CFLAGS		=	-std=c++11 -Wall
LIB     	=   -lrt -lpthread

#typing make will by default build the consumer and producer executables
all: consumer producer

#because both executables require CFLAGS and LIB, we put them in this way
consumer:	consumer.c
	g++ $(CFLAGS) consumer.c -o consumer $(LIB)

producer:	producer.c
	g++ $(CFLAGS) producer.c -o producer $(LIB)

clean:	
	-rm -f consumer producer